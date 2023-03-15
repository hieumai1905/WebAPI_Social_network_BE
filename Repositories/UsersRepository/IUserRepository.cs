using Web_Social_network_BE.Models;

namespace Web_Social_network_BE.Repositories.UserRepository
{
    public interface IUserRepository : IGeneralRepository<User, string>
    {
        Task<UsersInfo> GetUserInfoAsync(string userInfoId);
        Task<User> GetByEmail(string email);
        Task<User> GetByPhone(string phone);
    }
}
