using Web_Social_network_BE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.VisualBasic;

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
                return await _context.Posts.Where(context => context.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while get all post by user with id {userId}. ", ex);
            }
        }
        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            try
            {
                return await _context.Posts.OrderByDescending(x => x.CreateAt).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while get all post.", ex);
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
                throw new Exception($"An error occurred while get post with id {postId}", ex);
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
                throw new Exception($"An error occurred while add new post with id {entity.PostId}", ex);
            }
        }
        public async Task UpdateAsync(Post entity)
        {
            try
            {
                var postToUpdate = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(x => x.PostId == entity.PostId);
                if (postToUpdate == null)
                {
                    throw new ArgumentException("Error postToUpdate is not exist");
                }
                _context.Posts.Update(entity);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating post with id {entity.PostId}.", ex);
            }
        }
        public async Task DeleteAsync(string postId)
        {
            try
            {
                Post postToDelete = await _context.Posts.FirstOrDefaultAsync(context => context.PostId == postId);
                if (postToDelete == null)
                {
                    throw new ArgumentException("Error postToDelete is not exist");
                }
                _context.Posts.Remove(postToDelete);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting post with id {postId}.", ex);
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