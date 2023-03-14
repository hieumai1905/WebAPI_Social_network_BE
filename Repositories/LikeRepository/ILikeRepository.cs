using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories;

namespace Web_Social_network_BE.Repositories.LikeRepository
{
    public interface ILikeRepository: IGeneralRepository<Like, long>
    {
        Task<Post> AddLikeByPostIdAsync(string postId, Like entity);
        Task<Like> DeleteLikeByPostIdAsync(string postId, long likeId);
        Task<Comment> AddLikeByCommentIdAsync(long commentId, Like entity);
        Task<Like> DeleteLikeByCommentIdAsync(long commentId, long likeId);
    }
}
