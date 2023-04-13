using Web_Social_network_BE.Models;

namespace Web_Social_network_BE.Repositories.ReportRepository
{
    public interface IReportRepository: IGeneralRepository<Report, string>
    {
        Task<IEnumerable<Report>> GetAllReportByPostId(string postId);
        Task<Report> AddReportByPostId(string postId, Report entity);
        //Task<UsersInfo> BanUserByUserId(string userId);
    }
}
