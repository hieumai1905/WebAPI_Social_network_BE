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
			string[] keyWord = new string[] { "chien tranh", "banh mi", "sua dac", "an khuya" };
			foreach (string key in keyWord)
			{
				if (post.Content.Contains(key))
				{
					return BadRequest();
				}
			}
			try
			{
				string postId = Guid.NewGuid().ToString();
				DateTime createAt = DateTime.Now;
				post.PostId = postId;
				post.CreateAt = createAt;
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
			string[] keyWord = new string[] { "chien tranh", "banh mi", "sua dac", "an khuya" };
			foreach (string key in keyWord)
			{
				if (post.Content.Contains(key))
				{
					return BadRequest();
				}
			}
			try
			{
				post.CreateAt = DateTime.Now;
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
        public async Task<IActionResult> GetPostById(string id_post)
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

        [HttpGet("users/{id_user}")]
        public async Task<IActionResult> GetAllPostByUserId(string id_user)
        {
            try
            {
                var post = await _postRepository.GetAllAsyncByUserId(id_user);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id_post}/likes/users")]
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