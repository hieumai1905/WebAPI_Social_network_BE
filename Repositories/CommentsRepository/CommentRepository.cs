using Web_Social_network_BE.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Web_Social_network_BE.Repositories.CommentRepository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly SocialNetworkN01Ver2Context _context;
        public CommentRepository(SocialNetworkN01Ver2Context context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Comment>> GetAllCommentByPostIdAsync(string postId)
        {
            try
            {
                return await _context.Comments.Where(u => u.PostId == postId).ToListAsync();
            }
            catch
            {
                throw new ArgumentException($"Post with id {postId} does not exist");
            }
        }
        public async Task<Comment> AddCommentByPostIdAsync(string postId, Comment entity)
        {
            var postToComment = await _context.Posts.FirstOrDefaultAsync(u => u.PostId == postId).ConfigureAwait(false);
            if (postToComment == null)
            {
                throw new ArgumentException($"Post with id {postId} does not exist");
            }
            _context.Comments.Add(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return entity;
        }

        public async Task<Comment> GetByIdAsync(long key)
        {
            try
            {
                return await _context.Comments.FirstOrDefaultAsync(u => u.CommentId == key);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting comment with id {key}.", ex);
            }
        }
        public async Task<Comment> UpdateCommentByPostIdAsync(string postId, Comment entity)
        {
            var postToComment = await _context.Posts
                .FirstOrDefaultAsync(u => u.PostId == postId).ConfigureAwait(false);

            if (postToComment == null)
            {
                throw new ArgumentException($"Post with id {postId} does not exist");
            }
            var commentToUpdate = await _context.Comments.FindAsync(entity.CommentId);

            if (commentToUpdate == null)
            {
                throw new ArgumentException($"Comment with id {entity.CommentId} does not exist");
            }
            _context.Comments.Update(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return entity;
        }

        public async Task<Comment> DeleteCommentByPostIdAsync(string postId, long commentId)
        {
            var postToDelete = await _context.Comments
                .FirstOrDefaultAsync(u => u.PostId == postId).ConfigureAwait(false);

            if (postToDelete == null)
            {
                throw new ArgumentException($"Post with id {postId} does not exist");
            }
            var commentToDelete = await _context.Comments.FindAsync(commentId);

            if (commentToDelete == null)
            {
                throw new ArgumentException($"Comment with id {commentId} does not exist");
            }
            _context.Comments.Remove(commentToDelete);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return postToDelete;
        }

        public async Task UpdateAsync(Comment entity)
        {
            try
            {
                var commentToUpdate = await _context.Comments.FindAsync(entity.CommentId);

                if (commentToUpdate == null)
                {
                    throw new ArgumentException($"Comment with id {entity.CommentId} does not exist");
                }

                _context.Comments.Update(entity);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating comment with id {entity.CommentId}.", ex);
            }
        }
        //Khong Dung Den
        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            try
            {
                return await _context.Comments.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while get all comment.", ex);
            }
        }

        public Task<Comment> AddAsync(Comment entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long key)
        {
            throw new NotImplementedException();
        }
    }
}
