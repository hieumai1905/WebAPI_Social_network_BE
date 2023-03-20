using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.PostRepository;

namespace Web_Social_network_BE.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPost()
        {
            var post = await _postRepository.GetAllAsync();
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Post post)
        {
            var postId = await _postRepository.AddAsync(post);
            return Ok(postId);
        }
        [HttpPut("{id_post}")]
        public async Task<IActionResult> Update([FromBody] Post post)
        {
            await _postRepository.UpdateAsync(post);
            return Ok();
        }
        [HttpDelete("{id_post}")]
        public async Task<IActionResult> Delete(string id_post)
        {
            await _postRepository.DeleteAsync(id_post);
            return NoContent();
        }

        [HttpGet("{id_post}")]
        public async Task<IActionResult> GetAllPostById(string id_post)
        {
            var post = await _postRepository.GetByIdAsync(id_post);
            return Ok(post);
        }

        [HttpGet("v1/api/posts/users/{id_user}")]
        public async Task<IActionResult> GetAllPostByUser(string id_user)
        {
            var post = await _postRepository.GetAllAsyncByUserId(id_user);
            return Ok(post);
        }

        [HttpGet("v1/api/posts/{id_post}/likes/users")]
        public async Task<IActionResult> GetAllUsersLikePost(string id_post)
        {
            var user = await _postRepository.GetAllUserLikeAsync(id_post);
            return Ok(user);
        }
    }
}
