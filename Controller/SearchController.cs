using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.UserRepository;

namespace Web_Social_network_BE.Controller
{
	[Route("v1/api/[Controller]")]
	[ApiController]
	public class SearchController : ControllerBase
	{
		private IUserRepository _userRepository;

		public SearchController(IUserRepository userRepository)
		{
			this._userRepository = userRepository;
		}
		[HttpGet("people/userInfoId")]
		public async Task<IActionResult> GetUserInfo(string q)
		{
			try
			{
				var user = await _userRepository.GetUserInfoAsync(q);
				return Ok(user);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		[HttpGet("people/email")]
		public async Task<IActionResult> GetByEmail(string q)
		{
			try
			{
				var user = await _userRepository.GetByEmail(q);
				return Ok(user);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		[HttpGet("people/phone")]
		public async Task<IActionResult> GetByPhone(string q)
		{
			try
			{
				var user = await _userRepository.GetByPhone(q);
				return Ok(user);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
