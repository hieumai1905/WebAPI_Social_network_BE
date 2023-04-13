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
                if (user.UserInfo.Status == "LOOK")
                    return BadRequest("Your account is locked");
                if (user != null)
                {
                    _session.SetString("UserId", user.UserId);
                    _session.SetString("UserRole", user.UserInfo.UserRole);
                    _session.SetString("UserStatus", user.UserInfo.Status);
                    Response.Cookies.Append("UserId", user.UserId);
                    Response.Cookies.Append("UserRole", user.UserInfo.UserRole);
                    Response.Cookies.Append("UserStatus", user.UserInfo.Status);
                    _session.Remove(account.Email);
                    return Ok(user);
                }
                return BadRequest("Email or password is incorrect");
            }
            catch (Exception ex)
            {
                var userLogin =  await _userRepository.GetByEmail(account.Email);
                if (userLogin != null)
                {
                    var loginCount = _session.GetString(account.Email);
                    if (Convert.ToInt32(loginCount) > 5)
                    {
                        var userLoginInfo = await _userRepository.GetInformationUser(userLogin.UserId);
                        userLoginInfo.UserInfo.Status = "INACTIVE";
                        await _userRepository.UpdateAsync(userLoginInfo);
                        return BadRequest("Your account is locked");
                    }

                    if (loginCount == null)
                    {
                        _session.SetString(account.Email, "1");
                    }
                    else
                    {
                        _session.SetString(account.Email, (Convert.ToInt32(loginCount) + 1).ToString());
                    }
                }
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

                if (request.RegisterAt.AddMinutes(1) < DateTime.Now)
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

        [HttpPost("forgot-password/{email}")]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            try
            {
                var account = await _userRepository.GetByEmail(email);
                if (account == null)
                {
                    return BadRequest("Email don't exist");
                }

                var requestExist = await _requestCodeRepository.GetByEmail(email);
                var code = new Random().Next(100000, 999999);
                if (requestExist == null || requestExist.CodeType != "FORGOT_PASSWORD")
                {
                    Request request = new Request()
                    {
                        RegisterAt = DateTime.Now,
                        CodeType = "FORGOT_PASSWORD",
                        RequestCode = code,
                        Email = email
                    };
                    try
                    {
                        await _requestCodeRepository.CleanRequestCode();
                        await _requestCodeRepository.AddAsync(request);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred while adding the request: {ex.Message}");
                    }
                }
                else
                {
                    var request = await _requestCodeRepository.RefreshCode(email);
                    code = request.RequestCode;
                }

                try
                {
                    Mail.SendMail(email, "Confirm code!", code.ToString(),
                        email);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while sending the email: {ex.Message}");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("forgot-password/confirm-code")]
        public async Task<ActionResult> ConfirmForgotPassword(string code, [FromBody] string email)
        {
            try
            {
                var account = await _userRepository.GetByEmail(email);
                if (account == null)
                {
                    return BadRequest("Email don't exist");
                }

                var user = await _userRepository.GetInformationUser(account.UserId);
                var request = await _requestCodeRepository.GetByEmail(email);

                if (request.RequestCode.ToString() != code)
                {
                    return BadRequest("Code is incorrect");
                }

                if (request.CodeType != "FORGOT_PASSWORD")
                {
                    return BadRequest("Code is incorrect");
                }

                if (request.RegisterAt.AddMinutes(1) < DateTime.Now)
                {
                    return BadRequest("Code is expired");
                }

                _session.SetString("UserId", user.UserId);
                _session.SetString("UserRole", user.UserInfo.UserRole);
                _session.SetString("UserStatus", user.UserInfo.Status);
                Response.Cookies.Append("UserId", user.UserId);
                Response.Cookies.Append("UserRole", user.UserInfo.UserRole);
                Response.Cookies.Append("UserStatus", user.UserInfo.Status);
                await _requestCodeRepository.DeleteAsync(request.RegisterId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] string changePassword)
        {
            try
            {
                var userId = _session.GetString("UserId");
                if (userId == null)
                    return BadRequest("You must be logged in to change your password");
                var user = await _userRepository.GetInformationUser(userId);
                if (MD5Hash.GetHashString(changePassword) == user.UserInfo.Password)
                {
                    return BadRequest("Don't use the same password");
                }

                user.UserInfo.Password = MD5Hash.GetHashString(changePassword);
                await _userRepository.UpdateAsync(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}