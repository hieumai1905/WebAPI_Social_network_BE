using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.CommentRepository;
using Web_Social_network_BE.Repositories.UserRepository;

namespace Web_Social_network_BE.Controller
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        public CommentController(ICommentRepository commentRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }
        [HttpGet("posts/{postId}")]
        public async Task<IActionResult> GetAllCommentByPostId(string postId)
        {
            try
            {
                var allComment = await _commentRepository.GetAllCommentByPostIdAsync(postId);
                return Ok(allComment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting all comment: {ex.Message}");
            }
        }
        [HttpPost("posts/{postId}")]
        public async Task<IActionResult> AddCommentByPostId(string postId, [FromBody] Comment comment)
        {
            var userStatus = _session.GetString("UserStatus");
            if (userStatus == "BAN")
            {
                return StatusCode(403, "Forbidden");
            }
            string[] keyWord = new string[] { "chien tranh", "banh mi", "sua dac", "an khuya" };
            foreach (string key in keyWord)
            {
                if (comment.Content.Contains(key))
                {
                    return BadRequest();
                }
            }
            try
            {
                var addComment = await _commentRepository.AddCommentByPostIdAsync(postId, comment);
                return Ok(addComment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding new comment: {ex.Message}");
            }
        }
        [HttpGet("{postId}/{commentId}")]
        public async Task<IActionResult> GetCommentById(long commentId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);

            if (comment == null)
            {
                return NotFound($"Comment with id {commentId} not found");
            }
            return Ok(comment);
        }
        [HttpPut("posts/{postId}/{commentId}")]
        public async Task<IActionResult> UpdateCommentById(string postId, [FromBody] Comment comment)
        {
            var userStatus = _session.GetString("UserStatus");
            if (userStatus == "BAN")
            {
                return StatusCode(403, "Forbidden");
            }
            string[] keyWord = new string[] { "chien tranh", "banh mi", "sua dac", "an khuya" };
            foreach (string key in keyWord)
            {
                if (comment.Content.Contains(key))
                {
                    return BadRequest();
                }
            }
            if (postId != comment.PostId)
            {
                return BadRequest("Post id in the request body does not match the id in the URL");
            }

            await _commentRepository.UpdateAsync(comment);
            return Ok();
        }
        [HttpDelete("posts/{postId}/{commentId}")]
        public async Task<IActionResult> DeleteCommentById(string postId, long commentId)
        {
            try
            {
                var deletedComment = await _commentRepository.DeleteCommentByPostIdAsync(postId, commentId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting comment: {ex.Message}");
            }
        }
        [HttpDelete("posts/{postId}/comment")]
        public async Task<IActionResult> DeleteAllCommentByPostId(string postId)
        {
            try
            {
                await _commentRepository.DeleteAllCommentsByPostId(postId);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting comment: {ex.Message}");
            }
        }
    }
}
