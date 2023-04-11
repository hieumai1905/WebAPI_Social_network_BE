using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Web_Social_network_BE.Handle;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.RequestModel;
namespace Web_Social_network_BE.Repositories.NotificationsReponsitory
{
	public class NotificationRepository : INotificationRepository
	{
		private readonly SocialNetworkN01Ver2Context _context;

		public NotificationRepository(SocialNetworkN01Ver2Context context)
		{
			_context = context;
		}
		public async Task<Notification> AddAsync(Notification entity)
		{
			try
			{
				_context.Notifications.Add(entity);
				await _context.SaveChangesAsync().ConfigureAwait(false);
				return entity;
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while adding notification {entity.NotificationId}.{ex}");
			}
		}

		public async Task DeleteAsync(string key)
		{
			try
			{
				var notificationId = await _context.Notifications.FindAsync(key);

				if (notificationId == null)
				{
					throw new ArgumentException($"Notification with id {key} does not exist");
				}
				else
				{
					_context.Notifications.Remove(notificationId);
					await _context.SaveChangesAsync();
				}
				
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while deleting notification with id {key}.{ex}");
			}
		}

		public async Task<IEnumerable<Notification>> GetAllAsync()
		{
			try
			{
				return await _context.Notifications.ToListAsync();
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while getting all notification.{ex}");
			}
		}

        public async Task<IEnumerable<Notification>> GetAllAsyncByUserTargetId(string userTargetId)
        {
            try
            {
                return await _context.Notifications.Where(context => context.UserTargetId == userTargetId).OrderByDescending(x => x.NotificationsAt).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting all notification.{ex}");
            }
        }

        public async Task<Notification> GetByIdAsync(string key)
		{
			try
			{
				return await _context.Notifications.FindAsync(key);
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while getting notification with id {key}.{ex}");
			}
		}



        public async Task UpdateAsync(Notification entity)
		{
			try
			{
				var notiUpdate =
					await _context.Notifications.AsNoTracking().FirstOrDefaultAsync(x => x.NotificationId == entity.NotificationId);

				if (notiUpdate == null)
				{
					throw new ArgumentException($"notification with id {entity.NotificationId} does not exist");
				}

				_context.Notifications.Update(entity);
				await _context.SaveChangesAsync().ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while updating notification with id {entity.NotificationId}.{ex}");
			}
		}
	}
}
