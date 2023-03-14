
using Web_Social_network_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Chaythurepo.Repository.LikeRepository
{
    public class LikeRepository : ILikeRepository
    {
        private readonly SocialNetworkN01Context _context;
        public LikeRepository(SocialNetworkN01Context context)
        {
            _context = context;
        }

        public async Task<Post> AddLikeByPostIdAsync(string postId, Like entity)
        {
           
                var postToLike = await _context.Posts
                .FirstOrDefaultAsync(u => u.PostId == postId).ConfigureAwait(false);

                if(postToLike == null)
                {
                    throw new ArgumentException($"Post with id {postId} does not exist");
                }
                _context.Likes.Add(entity);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                
                return postToLike;
        }
        public async Task<Like> DeleteLikeByPostIdAsync(string postId, long likeId)
        {

            var postToUnLike = await _context.Likes
            .FirstOrDefaultAsync(u => u.PostId == postId).ConfigureAwait(false);

            if (postToUnLike == null)
            {
                throw new ArgumentException($"Post with id {postId} does not exist");
            }
            var likeToDelete = await _context.Likes.FindAsync(likeId);
            if (likeToDelete == null)
            {
                throw new ArgumentException($"Like with id {likeId} does not exist");
            }
            _context.Likes.Remove(likeToDelete);
            await _context.SaveChangesAsync();

            return postToUnLike;
        }
        public async Task<Comment> AddLikeByCommentIdAsync(long commentId, Like entity)
        {
            var commentToLike = await _context.Comments
                .FirstOrDefaultAsync(u => u.CommentId == commentId).ConfigureAwait(false);

            if (commentToLike == null)
            {
                throw new ArgumentException($"Comment with id {commentId} does not exist");
            }
            _context.Likes.Add(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return commentToLike;
        }

        public async Task<Like> DeleteLikeByCommentIdAsync(long commentId, long likeId)
        {
            var commentToUnLike = await _context.Likes
             .FirstOrDefaultAsync(u => u.CommentId == commentId).ConfigureAwait(false);

            if (commentToUnLike == null)
            {
                throw new ArgumentException($"Comment with id {commentId} does not exist");
            }
            var likeToDelete = await _context.Likes.FindAsync(likeId);
            if (likeToDelete == null)
            {
                throw new ArgumentException($"Like with id {likeId} does not exist");
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
    }
}
