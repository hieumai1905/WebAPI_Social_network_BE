using Microsoft.AspNetCore.Mvc;
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

        public AccountController(IRequestCodeRepository requestCodeRepository, IUserRepository userRepository)
        {
            _requestCodeRepository = requestCodeRepository;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel account)
        {
            try
            {
                var user = await _userRepository.Login(account);
                if (user != null)
                {
                    return Ok(user);
                }

                return BadRequest("Email or password is incorrect");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] LoginModel account)
        {
            try
            {
                var user = await _userRepository.GetByEmail(account.Email);
                if (user != null)
                {
                    return BadRequest("Email is already in use");
                }

                var userInfoId = Guid.NewGuid().ToString();
                var newUser = new User
                {
                    UserId = Guid.NewGuid().ToString(),
                    FullName = "User",
                    Avatar = null,
                    UserInfoId = userInfoId,
                    UserInfo = new UsersInfo()
                    {
                        UserInfoId = userInfoId,
                        Password = account.Password,
                        Email = account.Email,
                        Dob = null,
                        Address = null,
                        Status = "INACTIVE",
                        UserRole = "USER_ROLE",
                        AboutMe = "",
                        CoverImage = null
                    }
                };
                var userToAdd = await _userRepository.AddAsync(newUser);
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
                    Mail.SendMail(account.Email, "Confirm code!", requestCode.ToString(),
                        account.Email);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while sending the email: {ex.Message}");
                }

                var requestToAdd = await _requestCodeRepository.AddAsync(request);
                return Ok(userToAdd);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}