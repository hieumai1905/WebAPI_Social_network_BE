using Microsoft.AspNetCore.Mvc;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.PostRepository;
using Web_Social_network_BE.Repositories.UserRepository;
using Web_Social_network_BE.RequestModel;

namespace Web_Social_network_BE.Controller;

[Route("v1/api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;

    public AdminController(IUserRepository userRepository, IPostRepository postRepository)
    {
        this._userRepository = userRepository;
        this._postRepository = postRepository;
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        try
        {
            var users = await _userRepository.GetAllWithInfoAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("users/admin")]
    public async Task<ActionResult<User>> CreateAdmin([FromBody] RegisterModel account)
    {
        try
        {
            var userToAdd = await _userRepository.Register(account);
            userToAdd.UserInfo.UserRole = "ADMIN_ROLE";
            userToAdd.UserInfo.Status = "ACTIVE";
            await _userRepository.UpdateAsync(userToAdd);
            return Ok(userToAdd);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
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

    [HttpPut("lock/{userId}")]
    public async Task<IActionResult> LookUser(string userId)
    {
        try
        {
            var user = await _userRepository.LockAsync(userId);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPut("un-lock/{userId}")]
    public async Task<IActionResult> UnLookUser(string userId)
    {
        try
        {
            var user = await _userRepository.UnLockAsync(userId);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("posts/{id_post}")]
    public async Task<IActionResult> DeletePost(string id_post)
    {
        try
        {
            await _postRepository.DeleteAsync(id_post);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}