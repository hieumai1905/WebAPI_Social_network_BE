using Microsoft.AspNetCore.Mvc;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.LikeRepository;

namespace Web_Social_network_BE.Controller
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeRepository _likeRepository;
        public LikeController(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }
        [HttpPost("posts/{postId}")]
        public async Task<IActionResult> AddLikeByPostId(string postId, [FromBody] Like like)
        {
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
    }
}
