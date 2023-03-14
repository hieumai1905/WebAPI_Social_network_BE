using Web_Social_network_BE.Models;
using Microsoft.EntityFrameworkCore;
namespace Web_Social_network_BE.Repositories.PostRepository
{
    public class PostRepository : IPostRepository
    {
        private readonly SocialNetworkN01Context _context;
        public PostRepository(SocialNetworkN01Context context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Post>> GetAllAsyncByUserId(string userId)
        {
            try
            {
                var posts = await _context.Posts.Where(x => x.UserId == userId).ToListAsync();
                return await _context.Posts.Where(x => x.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while get all post by user id {userId}. ", ex);
            }
        }
        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            try
            {
                return await _context.Posts.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while get all post. ", ex);
            }
        }
        public Task<Post> GetByIdAsync(string postId)
        {
            try
            {
                return _context.Posts.FirstOrDefaultAsync(context => context.PostId == postId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while get post id {postId}", ex);
            }
        }
        public async Task<Post> AddAsync(Post entity)
        {
            try
            {
                _context.Posts.Add(entity);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while add new post {entity.PostId}", ex);
            }
        }
        public async Task UpdateAsync(Post entity)
        {
            try
            {
                var postToUpdate = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(x => x.PostId == entity.PostId);
                if (postToUpdate == null)
                {
                    throw new ArgumentException("Error while update post");
                }
                _context.Posts.Update(entity);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating user with id {entity.PostId}.", ex);
            }
        }
        public async Task DeleteAsync(string userId)
        {
            try
            {
                List<Post> list = _context.Posts.Where(context => context.UserId == userId).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    _context.Posts.Remove(list[i]);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while delete post user {userId}.", ex);
            }
        }

        public async Task<IEnumerable<Like>> GetAllUserLikeAsync(string postId)
        {
            try
            {
                return await _context.Likes.Where(context => context.PostId == postId && context.CommentId == null).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while get all user like post", ex);
            }
        }
    }
}
