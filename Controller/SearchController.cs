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
		[HttpGet("people/name=/{userInfoId}")]
		public async Task<IActionResult> GetUserInfo(string userInfoId)
		{
			try
			{
				var userInfor = await _userRepository.GetUserInfoAsync(userInfoId);
				return Ok(userInfor);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		[HttpGet("people/email=/{email}")]
		public async Task<IActionResult> GetByEmail(string email)
		{
			try
			{
				var uEmail = await _userRepository.GetByEmail(email);
				return Ok(uEmail);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		[HttpGet("people/phone=/{phone}")]
		public async Task<IActionResult> GetByPhone(string phone)
		{
			try
			{
				var uPhone = await _userRepository.GetByPhone(phone);
				return Ok(uPhone);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}

