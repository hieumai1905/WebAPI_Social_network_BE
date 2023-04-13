using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.LikeRepository;
using Web_Social_network_BE.Repositories.UserRepository;

namespace Web_Social_network_BE.Controller
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        public LikeController(ILikeRepository likeRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _likeRepository = likeRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }
        [HttpPost("posts/{postId}")]
        public async Task<IActionResult> AddLikeByPostId(string postId, [FromBody] Like like)
        {
            var userStatus = _session.GetString("UserStatus");
            if (userStatus == "BAN")
            {
                return StatusCode(403, "Forbidden");
            }
            try
            {
                var addedLike = await _likeRepository.AddLikeByPostIdAsync(postId, like);
                return Ok(addedLike);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding like: {ex.Message}");
            }
        }
        [HttpPost("cmts/{commentId}")]
        public async Task<IActionResult> AddLikeByCommentId(long commentId, [FromBody] Like like)
        {
            var userStatus = _session.GetString("UserStatus");
            if (userStatus == "BAN")
            {
                return StatusCode(403, "Forbidden");
            }
            try
            {
                var addedLike = await _likeRepository.AddLikeByCommentIdAsync(commentId, like);
                return Ok(addedLike);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding like: {ex.Message}");
            }
        }
        [HttpDelete("posts/{postId}")]
        public async Task<IActionResult> DeleteLikeByPostId(string postId, string userId)
        {
            var userStatus = _session.GetString("UserStatus");
            if (userStatus == "BAN")
            {
                return StatusCode(403, "Forbidden");
            }
            try
            {
                var deletedLike = await _likeRepository.DeleteLikeByPostIdAsync(postId, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting like: {ex.Message}");
            }
        }
        [HttpDelete("cmts/{commentId}")]
        public async Task<IActionResult> DeleteLikeByCommenttId(long commentId, string userId)
        {
            var userStatus = _session.GetString("UserStatus");
            if (userStatus == "BAN")
            {
                return StatusCode(403, "Forbidden");
            }
            try
            {
                var deletedLike = await _likeRepository.DeleteLikeByCommentIdAsync(commentId, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting like: {ex.Message}");
            }
        }
        [HttpDelete("cmts/{postId}/{commentId}")]
        public async Task<IActionResult> DeleteAllLikeByCommentId(string postId, long commentId)
        {
            try
            {
                var deletedComment = await _likeRepository.DeleteAllLikeByCommentIdAsync(postId, commentId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting like: {ex.Message}");
            }
        }
        [HttpDelete("post/{postId}/likes")]
        public async Task<IActionResult> DeleteAllLikes(string postId)
        {
            try
            {
                await _likeRepository.DeleteAllLikeByPostId(postId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting like: {ex.Message}");
            }
        }
        [HttpGet("cmt/{commentId}/likes")]
        public async Task<IActionResult> GetAllLikeComment(long commentId)
        {
            try
            {
                var likeComment = await _likeRepository.GetAllLikeComment(commentId);
                return Ok(likeComment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting all comment's like: {ex.Message}");
            }
        }
    }
}
