using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.UserRepository;
using Web_Social_network_BE.RequestModel;

namespace Web_Social_network_BE.Controller
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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

        [HttpPost]
        public async Task<ActionResult<User>> Create(User user)
        {
            try
            {
                string userInfoId = Guid.NewGuid().ToString();
                user.UserId = Guid.NewGuid().ToString();
                user.UserInfoId = userInfoId;
                user.UserInfo.UserInfoId = userInfoId;
                user.UserInfo.Status = "INACTIVE";
                var newUser = await _userRepository.AddAsync(user);
                return CreatedAtAction(nameof(GetById), new { id = newUser.UserId }, newUser);
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
                var user = await _userRepository.GetInformationUser(id);
                if (user == null || changePassword.OldPassword == "" ||
                    user.UserInfo.Password != changePassword.OldPassword)
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
            if (id != user.UserId)
            {
                return BadRequest();
            }

            try
            {
                await _userRepository.UpdateAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}