using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Social_network_BE.Handle;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.UserRepository;
using Web_Social_network_BE.RequestModel;
using Web_Social_network_BE.Sockets.SendMails;

namespace Web_Social_network_BE.Controller
{
    [Route("v1/api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRequestCodeRepository _requestCodeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public UserController(IUserRepository userRepository, IRequestCodeRepository requestCodeRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            this._userRepository = userRepository;
            this._requestCodeRepository = requestCodeRepository;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}/profile")]
        public async Task<ActionResult<User>> GetFullInformationUser(string id)
        {
            try
            {
                var user = await _userRepository.GetInformationUser(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<User>> SearchUser(string q)
        {
            try
            {
                var user = await _userRepository.FindUserContent(q);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("profile-me")]
        public async Task<ActionResult<User>> GetFullInformationUserMe()
        {
            try
            {
                var userId = _session.GetString("UserId");
                var user = await _userRepository.GetInformationUser(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("change-password")]
        public async Task<IActionResult> UpdatePassword(ChangePasswordModel changePassword)
        {
            try
            {
                var userId = _session.GetString("UserId");
                var user = await _userRepository.GetInformationUser(userId);
                if (user == null || changePassword.OldPassword == "" ||
                    user.UserInfo.Password != MD5Hash.GetHashString(changePassword.OldPassword))
                {
                    return BadRequest("Old password is incorrect");
                }

                if (changePassword.NewPassword == "" || changePassword.NewPassword.Length < 6)
                    return BadRequest("Password must be at least 6 characters long");

                user.UserInfo.Password = MD5Hash.GetHashString(changePassword.NewPassword);
                await _userRepository.UpdateAsync(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] User user)
        {
            var userId = _session.GetString("UserId");
            if (userId != id || userId == null)
            {
                return StatusCode(403, "Forbidden");
            }

            if (id != user.UserId)
            {
                return BadRequest("Id is not match");
            }

            try
            {
                if (user.UserInfo.UserRole == "ADMIN_ROLE")
                {
                    var role = _session.GetString("UserRole");
                    if (role != "ADMIN_ROLE")
                        return StatusCode(403, "Forbidden");
                }

                await _userRepository.UpdateAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("change-email/{email}")]
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
                if (requestExist == null || requestExist.CodeType != "CHANGE_EMAIL")
                {
                    Request request = new Request()
                    {
                        RegisterAt = DateTime.Now,
                        CodeType = "CHANGE_EMAIL",
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

        [HttpPut("change_email/{code}")]
        public async Task<ActionResult> ConfirmForgotPassword(string code, [FromBody] ChangeEmailModel emailChange)
        {
            try
            {
                var account = await _userRepository.GetByEmail(emailChange.OldEmail);
                if (account == null)
                {
                    return BadRequest("Email don't exist");
                }

                var user = await _userRepository.GetInformationUser(account.UserId);
                if (user.UserInfo.Email == emailChange.NewEmail)
                {
                    return BadRequest(("Email is exist"));
                }

                var request = await _requestCodeRepository.GetByEmail(emailChange.OldEmail);

                if (request.RequestCode.ToString() != code)
                {
                    return BadRequest("Code is incorrect");
                }

                if (request.CodeType != "CHANGE_EMAIL")
                {
                    return BadRequest("Code is incorrect");
                }
                
                var checkEmail = await _userRepository.GetByEmail(emailChange.NewEmail);
                if(checkEmail != null)
                {
                    return BadRequest("Email is exist for another account");
                }

                if (request.RegisterAt.AddMinutes(1) < DateTime.Now)
                {
                    return BadRequest("Code is expired");
                }

                user.UserInfo.Email = emailChange.NewEmail;
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