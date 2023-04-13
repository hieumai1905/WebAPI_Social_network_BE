using Microsoft.EntityFrameworkCore;
using Web_Social_network_BE.Models;

namespace Web_Social_network_BE.Repositories.ReportRepository
{
    public class ReportRepository : IReportRepository
    {
        private readonly SocialNetworkN01Ver2Context _context;
        public ReportRepository(SocialNetworkN01Ver2Context context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Report>> GetAllReportByPostId(string postId)
        {
            try
            {
                return await _context.Reports.Where(u => u.PostId == postId).ToListAsync();
            }
            catch
            {
                throw new ArgumentException($"Report post with id {postId} does not exist");
            }
        }

        public async Task<Report> AddReportByPostId(string postId, Report entity)
        {
            var postToReport = await _context.Posts.FirstOrDefaultAsync(u => u.PostId == postId).ConfigureAwait(false);
            if (postToReport == null)
            {
                throw new ArgumentException($"Post with id {postId} does not exist");
            }
            _context.Reports .Add(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return entity;
        }

        //public async Task<UsersInfo> BanUserByUserId(string userId)
        //{
        //    var userToBan = _context.UsersInfos.FirstOrDefaultAsync(u => u.Use)
        //}

       

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            try
            {
                return await _context.Reports.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while get all report.", ex);
            }
        }

        


        //Khong Dung
        public Task<Report> GetByIdAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Report entity)
        {
            throw new NotImplementedException();
        }
        public Task<Report> AddAsync(Report entity)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(string key)
        {
            throw new NotImplementedException();
        }
    }
}
