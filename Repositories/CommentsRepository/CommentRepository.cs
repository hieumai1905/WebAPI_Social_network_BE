using Web_Social_network_BE.Models;

namespace Web_Social_network_BE.Repositories.CommentsRepository
{
    public class CommentRepository : ICommentRepository
    {
        public Task<Comment> AddAsync(Comment entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long key)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Comment>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Comment> GetByIdAsync(long key)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Comment entity)
        {
            throw new NotImplementedException();
        }
    }
}
