using Web_Social_network_BE.Models;


namespace Web_Social_network_BE.Repositories.NotificationsReponsitory
{
	public interface INotificationRepository: IGeneralRepository<Notification, string>
	{
        Task<IEnumerable<Notification>> GetAllAsyncByUserTargetId(string userTargetId);
        Task<int> GetCountNotification(string userTargetId);
        
    }
}
