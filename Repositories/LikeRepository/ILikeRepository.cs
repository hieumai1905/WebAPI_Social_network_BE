using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories;

namespace Web_Social_network_BE.Repositories.LikeRepository
{
    public interface ILikeRepository : IGeneralRepository<Like, long>
    {
        Task<Like> AddLikeByPostIdAsync(string postId, Like entity);
        Task<Like> DeleteLikeByPostIdAsync(string postId, string userId);
        Task<Like> AddLikeByCommentIdAsync(long commentId, Like entity);
        Task<Like> DeleteLikeByCommentIdAsync(long commentId, string userId);
        Task<Like> DeleteAllLikeByCommentIdAsync(string postId, long commentId);
        Task DeleteAllLikeByPostId(string postId);
        Task<IEnumerable<Like>> GetAllLikeComment(long commentId);
    }
}
