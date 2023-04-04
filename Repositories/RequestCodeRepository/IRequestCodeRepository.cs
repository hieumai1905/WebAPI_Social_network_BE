using Web_Social_network_BE.Models;

namespace Web_Social_network_BE.Repositories.UserRepository
{
    public interface IRequestCodeRepository : IGeneralRepository<Request, long>
    {
        public Task<Request> GetByEmail(string email);
        public Task CleanRequestCode();
        public Task<int> GetCountRequest();
        Task<Request> RefreshCode(string email);
    }
}