using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
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
            try
            {
                var post = await _postRepository.GetAllAsync();
                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Post post)
        {
            try
            {
                string postId = Guid.NewGuid().ToString();
                post.PostId = postId;
                var newPost = await _postRepository.AddAsync(post);
                return Ok(newPost);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPut("{id_post}")]
        public async Task<IActionResult> Update(string id_post, [FromBody] Post post)
        {
            if (id_post != post.PostId)
            {
                return BadRequest();
            }
            try
            {
                await _postRepository.UpdateAsync(post);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("{id_post}")]
        public async Task<IActionResult> Delete(string id_post)
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

        [HttpGet("{id_post}")]
        public async Task<IActionResult> GetAllPostById(string id_post)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(id_post);
                if (post == null)
                {
                    return NotFound();
                }
                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("v1/api/posts/users/{id_user}")]
        public async Task<IActionResult> GetAllPostByUser(string id_user)
        {
            try
            {
                var post = await _postRepository.GetAllAsyncByUserId(id_user);
                if (post.IsNullOrEmpty())   //Trường hợp người dùng không tồn tại thì đúng, nhưng nếu người dùng không đăng bài thì sẽ không hợp lí
                {
                    return NotFound();
                }
                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("v1/api/posts/{id_post}/likes/users")]
        public async Task<IActionResult> GetAllUsersLikePost(string id_post)
        {
            try
            {
                var user = await _postRepository.GetAllUserLikeAsync(id_post);
                if (user.IsNullOrEmpty())
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}