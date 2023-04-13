using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Web_Social_network_BE.Handle;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.NotificationsReponsitory;
using Web_Social_network_BE.Repositories.PostRepository;
using Web_Social_network_BE.Repositories.UserRepository;
using Web_Social_network_BE.RequestModel;

namespace Web_Social_network_BE.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class NotificationController : ControllerBase
	{
		private readonly INotificationRepository _notificationRepository;

		public NotificationController(INotificationRepository notificationRepository)
		{
			_notificationRepository = notificationRepository;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllNotification()
		{
			try
			{
				var noti = await _notificationRepository.GetAllAsync();
				return Ok(noti);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetNotificationById(string id)
		{
			try
			{
				var noti = await _notificationRepository.GetByIdAsync(id);
				if (noti == null)
				{
					return NotFound();
				}
				return Ok(noti);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		[HttpGet("userTarget/{id}")]
		public async Task<IActionResult> GetNotificationByUserTarget(string id)
		{
            try
            {
				var userTarget = await _notificationRepository.GetAllAsyncByUserTargetId(id);
                return Ok(userTarget);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
		[HttpGet("count/{id}")]
		public async Task<IActionResult> GetCountNotification(string id)
		{
			try
			{
				var userTarget = await _notificationRepository.GetCountNotification(id);
				return Ok(userTarget);
			}
			catch(Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteNotificationById(string id)
		{
			try
			{
				await _notificationRepository.DeleteAsync(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		[HttpPost]
		public async Task<IActionResult> AddNotificationAddFriend([FromBody] Notification noti)
		{
			try
			{
				string notiId = Guid.NewGuid().ToString();
				DateTime createAt = DateTime.Now;
				string status = "NEW";
				noti.NotificationId = notiId;
				noti.NotificationsAt = createAt;
				noti.Status = status;
				var newNotification = await _notificationRepository.AddAsync(noti);
				return Ok(newNotification);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateNotificationByUserTargetId( [FromBody] Notification noti)
		{
			try
			{
                    await _notificationRepository.UpdateAsync(noti);
                    return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
