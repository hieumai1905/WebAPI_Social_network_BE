using Web_Social_network_BE.Models;
using Web_Social_network_BE.RequestModel;

namespace Web_Social_network_BE.Repositories.UserRepository
{
    public interface IUserRepository : IGeneralRepository<User, string>
    {
        Task<UsersInfo> GetUserInfoAsync(string userInfoId);
        Task<User> GetByEmail(string email);
        Task<User> GetByPhone(string phone);
        Task<User> GetInformationUser(string idUser);
        Task<User> Login(LoginModel account);
        Task<User> Register(RegisterModel account);
        Task<IEnumerable<User>> FindUserContent(string content);
        Task<IEnumerable<User>> GetAllWithInfoAsync();

        Task<User> LockAsync(string key);
        Task<User> UnLockAsync(string key);
    }
}