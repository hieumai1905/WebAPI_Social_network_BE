using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.PostRepository;
using Web_Social_network_BE.Repositories.UserRepository;

namespace Web_Social_network_BE.Controllers
{
    [Route("v2/api/[controller]")]
    [ApiController]

    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ISession _session;

		public PostsController(IPostRepository postRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
		{
			this._postRepository = postRepository;
            _userRepository = userRepository;
			_httpContextAccessor = httpContextAccessor;
			_session = _httpContextAccessor.HttpContext.Session;
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
        [HttpGet("home/posts")]
        public async Task<IActionResult> GetAllForHome()
        {
			try
			{
                var userId = _session.GetString("UserId");
				var post = await _postRepository.GetAllPostForHomeAsync(userId);
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
			string postId = Guid.NewGuid().ToString();
			post.PostId = postId;
			post.CreateAt = DateTime.Now;

			var userId = _session.GetString("UserId");
            var userStatus = _session.GetString("UserStatus");
			//Nếu không phải người dùng thì không được đăng bài -->  Phòng chống nghệ thuật hắc cơ
			if (userId != post.UserId || userId == null)
			{
				return StatusCode(401, "Unauthorized");
			}
			//Nếu người dùng bị ban thì không được đăng bài
			if (userStatus == "BAN")
            {
                return StatusCode(403, "Forbidden");
            }
            //Nếu spam thì  BAN
            bool checkSpam = true;
            var checkPostSpam = await _postRepository.GetAllAsyncByUserId(userId);
            if (checkPostSpam.Count() < 3) checkSpam = false;
            foreach (var postCheck in checkPostSpam.Take(3))
            {
                if (post.Content != postCheck.Content || postCheck.CreateAt.Day != post.CreateAt.Day)
                {
                    checkSpam = false;
                    break;
                }
            }
            if (checkSpam)
            {
                await _userRepository.BanAsync(userId);
                _session.SetString("UserStatus", "BAN");
                return StatusCode(403, "Forbidden");
			}
            //Không cho đăng bài viết vi phạm
			string[] keyWord = new string[] { "chien tranh", "banh mi", "sua dac", "an khuya" };
			foreach (string key in keyWord)
			{
				if (post.Content.Contains(key))
				{
					return StatusCode(400, "Bad Request");
				}
			}
			try
			{
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
			var userId = _session.GetString("UserId");
			var userStatus = _session.GetString("UserStatus");
			//Nếu không phải người dùng thì không được đăng bài --> Phòng chống nghệ thuật hắc cơ
			if (userId != post.UserId || userId == null)
			{
				return StatusCode(401, "Unauthorized");
			}
			//Nếu người dùng bị ban thì không được đăng bài
			if (userStatus == "BAN")
			{
				return StatusCode(403, "Forbidden");
			}
			string[] keyWord = new string[] { "chien tranh", "banh mi", "sua dac", "an khuya" };
			foreach (string key in keyWord)
			{
				if (post.Content.Contains(key))
				{
					return StatusCode(400, "Bad Request"); ;
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
            var postToDelete = await _postRepository.GetByIdAsync(id_post);
			var userId = _session.GetString("UserId");
			var userRole = _session.GetString("UserRole");
            //Nếu không phải người dùng thì không được đăng bài --> Phòng chống nghệ thuật hắc cơ
            var darkerSecurity = true;
            if (userId == postToDelete.UserId || userRole == "ADMIN_ROLE")
			{
				darkerSecurity = false;
			}
            if (darkerSecurity)
            {
				return StatusCode(401, "Unauthorized");
			}
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
		[HttpGet("month/posts")]
        public async Task<IActionResult> GetAllPostInThisMonth()
        {
            try
            {
                var postInMonth = await _postRepository.GetAllInMonthAsync();
                return Ok(postInMonth);
			}
            catch (Exception ex)
            {
				return StatusCode(500, ex.Message);
			}
        }
	}
}