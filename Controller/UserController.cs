using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Social_network_BE.Handle;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.UserRepository;
using Web_Social_network_BE.RequestModel;

namespace Web_Social_network_BE.Controller
{
    [Route("v1/api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public UserController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            this._userRepository = userRepository;
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

                user.UserInfo.Password = MD5Hash.GetHashString(user.UserInfo.Password);
                await _userRepository.UpdateAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}