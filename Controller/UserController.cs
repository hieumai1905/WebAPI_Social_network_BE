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
                user.UserInfo.Password = null;
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{id}/change-password")]
        public async Task<IActionResult> UpdatePassword(string id, ChangePasswordModel changePassword)
        {
            try
            {
                var userId = _session.GetString("UserId");
                if (userId != id || userId == null)
                {
                    return StatusCode(403, "Forbidden");
                }

                var user = await _userRepository.GetInformationUser(id);
                if (user == null || changePassword.OldPassword == "" ||
                    user.UserInfo.Password != MD5Hash.GetHashString(changePassword.OldPassword))
                {
                    return BadRequest();
                }

                user.UserInfo.Password = changePassword.NewPassword;
                await _userRepository.UpdateAsync(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User user)
        {
            var userId = _session.GetString("UserId");
            if (userId != id || userId == null)
            {
                return StatusCode(403, "Forbidden");
            }

            if (id != user.UserId)
            {
                return BadRequest();
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