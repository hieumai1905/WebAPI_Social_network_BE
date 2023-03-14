using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories;

namespace Web_Social_network_BE.Repositories.CommentRepository
{
    public interface ICommentRepository: IGeneralRepository<Comment, long>
    {
        
        Task<IEnumerable<Comment>> GetAllCommentByPostIdAsync(string postId);
        Task<Post> AddCommnetByPostIdAsync(string postId, Comment entity);
        Task<Post> UpdateCommentByPostIdAsync(string postId, Comment entity);
        Task<Comment> DeleteCommentByPostIdAsync(string postId, long commentId);
        
    }
}
