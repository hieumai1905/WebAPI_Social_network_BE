using Microsoft.AspNetCore.Mvc;
using Web_Social_network_BE.Handle;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.UserRepository;
using Web_Social_network_BE.RequestModel;
using Web_Social_network_BE.Sockets.SendMails;

namespace Web_Social_network_BE.Controller
{
    [Route("v1/api/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IRequestCodeRepository _requestCodeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public AccountController(IRequestCodeRepository requestCodeRepository, IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _requestCodeRepository = requestCodeRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel account)
        {
            try
            {
                var user = await _userRepository.Login(account);
                if (user != null)
                {
                    _session.SetString("UserId", user.UserId);
                    _session.SetString("UserRole", user.UserInfo.UserRole);
                    _session.SetString("UserStatus", user.UserInfo.Status);
                    return Ok(user);
                }

                return BadRequest("Email or password is incorrect");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                _session.Remove("UserId");
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel account)
        {
            try
            {
                var userToAdd = await _userRepository.Register(account);

                var requestCode = new Random().Next(100000, 999999);
                var request = new Request
                {
                    RegisterAt = DateTime.Now,
                    CodeType = "REGISTER",
                    RequestCode = requestCode,
                    Email = account.Email
                };

                try
                {
                    await _requestCodeRepository.CleanRequestCode();
                    var requestToAdd = await _requestCodeRepository.AddAsync(request);
                }
                catch (Exception ex)
                {
                    _userRepository.DeleteAsync(userToAdd.UserId);
                    throw new Exception($"An error occurred while adding the request: {ex.Message}");
                }

                try
                {
                    Mail.SendMail(account.Email, "Confirm code!", requestCode.ToString(),
                        account.Email);
                }
                catch (Exception ex)
                {
                    _userRepository.DeleteAsync(userToAdd.UserId);
                    _requestCodeRepository.DeleteAsync(request.RegisterId);
                    throw new Exception($"An error occurred while sending the email: {ex.Message}");
                }

                return Ok(userToAdd);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("register/confirm-code")]
        public async Task<ActionResult> ConfirmCode([FromQuery] string code, [FromBody] LoginModel account)
        {
            try
            {
                User user = null;
                try
                {
                    user = await _userRepository.Login(account);
                }
                catch (Exception ex)
                {
                    return BadRequest("Don't exist user with this email");
                }


                if (user.UserInfo.Status == "ACTIVE")
                {
                    return BadRequest("User is already active");
                }

                var request = await _requestCodeRepository.GetByEmail(account.Email);

                if (request.RequestCode.ToString() != code)
                {
                    return BadRequest("Code is incorrect");
                }

                if (request.CodeType != "REGISTER")
                {
                    return BadRequest("Code is incorrect");
                }

                if (request.RegisterAt.AddMinutes(5) < DateTime.Now)
                {
                    return BadRequest("Code is expired");
                }

                user.UserInfo.Status = "ACTIVE";
                await _userRepository.UpdateAsync(user);
                await _requestCodeRepository.DeleteAsync(request.RegisterId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}