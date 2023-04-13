
using Web_Social_network_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Web_Social_network_BE.Repositories.LikeRepository
{
    public class LikeRepository : ILikeRepository
    {
        private readonly SocialNetworkN01Ver2Context _context;
        public LikeRepository(SocialNetworkN01Ver2Context context)
        {
            _context = context;
        }

        public async Task<Like> AddLikeByPostIdAsync(string postId, Like entity)
        {

            var postToLike = await _context.Posts
            .FirstOrDefaultAsync(u => u.PostId == postId).ConfigureAwait(false);

            if (postToLike == null)
            {
                throw new ArgumentException($"Post with id {postId} does not exist");
            }
            _context.Likes.Add(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return entity;
        }
        public async Task<Like> DeleteLikeByPostIdAsync(string postId, string userId)
        {

            var postToUnLike = await _context.Likes
            .FirstOrDefaultAsync(u => u.PostId == postId).ConfigureAwait(false);

            if (postToUnLike == null)
            {
                throw new ArgumentException($"Post with id {postId} does not exist");
            }
            var likeToDelete = await _context.Likes.FirstOrDefaultAsync(u => u.UserId == userId);
            if (likeToDelete == null)
            {
                throw new ArgumentException($"Like with id {userId} does not exist");
            }
            _context.Likes.Remove(likeToDelete);
            await _context.SaveChangesAsync();

            return postToUnLike;
        }
        public async Task<Like> AddLikeByCommentIdAsync(long commentId, Like entity)
        {
            var commentToLike = await _context.Comments
                .FirstOrDefaultAsync(u => u.CommentId == commentId).ConfigureAwait(false);

            if (commentToLike == null)
            {
                throw new ArgumentException($"Comment with id {commentId} does not exist");
            }
            _context.Likes.Add(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return entity;
        }

        public async Task<Like> DeleteLikeByCommentIdAsync(long commentId, string userId)
        {
            var commentToUnLike = await _context.Likes
             .FirstOrDefaultAsync(u => u.CommentId == commentId).ConfigureAwait(false);

            if (commentToUnLike == null)
            {
                throw new ArgumentException($"Comment with id {commentId} does not exist");
            }
            var likeToDelete = await _context.Likes.FirstOrDefaultAsync(u => u.UserId == userId);
            if (likeToDelete == null)
            {
                throw new ArgumentException($"Like with id {userId} does not exist");
            }
            _context.Likes.Remove(likeToDelete);
            await _context.SaveChangesAsync();

            return commentToUnLike;
        }
        //Khong Dung Den
        public Task<IEnumerable<Like>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Like> GetByIdAsync(long key)
        {
            throw new NotImplementedException();
        }

        public Task<Like> AddAsync(Like entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Like entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long key)
        {
            throw new NotImplementedException();
        }

        public async Task<Like> DeleteAllLikeByCommentIdAsync(string postId, long commentId)
        {
            var postToDelete = await _context.Likes.FirstOrDefaultAsync(u => u.PostId == postId);
            if (postToDelete == null)
            {
                throw new ArgumentException($"Post with id {postId} does not exist");
            }
            var commentToDelete = await _context.Likes.Where(u => u.CommentId == commentId).ToListAsync();
            if (commentToDelete == null)
            {
                throw new ArgumentException($"Like with comment id {commentId} does not exist");
            }
            _context.Likes.RemoveRange(commentToDelete);
            await _context.SaveChangesAsync();
            return postToDelete;
        }

        public async Task DeleteAllLikeByPostId(string postId)
        {
            try
            {
                var likeToDelete = await _context.Likes.Where(u => u.PostId == postId).ToListAsync();
                _context.Likes.RemoveRange(likeToDelete);
                await _context.SaveChangesAsync().ConfigureAwait(false);

            }
            catch(Exception ex)
            {
                throw new Exception($"An error occurred while deleting post's likes with id {postId}.", ex);
            }
        }

        public async Task<IEnumerable<Like>> GetAllLikeComment(long commentId)
        {
            try
            {
                var likeComment = await _context.Likes.Where(u => u.CommentId == commentId).ToListAsync();
                return likeComment;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting comment's likes with id {commentId}.", ex);
            }
        }
    }
}
