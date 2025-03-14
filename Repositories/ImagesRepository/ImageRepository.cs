﻿using Web_Social_network_BE.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Web_Social_network_BE.Repositories.ImageRepository
{
    public class ImageRepository : IImageRepository
    {
        private readonly SocialNetworkN01Ver2Context _context;
        public ImageRepository(SocialNetworkN01Ver2Context context)
        {
            _context = context;
        }
        public async Task<Image> GetByIdAsync(string key)
        {
            try
            {
                return await _context.Images.FindAsync(key);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting image by using image id.", ex);
            }
        }
        public async Task<IEnumerable<Image>> GetAllAsync()
        {
            try
            {
                return await _context.Images.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting all image for post.", ex);
            }
        }
		public async Task<IEnumerable> GetImageByUserId(string userId)
		{
			List<Image> images = new List<Image>();
			List<Post> posts = await _context.Posts.Where(x => x.UserId == userId).ToListAsync();
			if (posts != null)
			{
				for (int i = 0; i < posts.Count; i++)
				{
					List<Image> imageinPost = await _context.Images.Where(x => x.PostId == posts[i].PostId).ToListAsync();
					foreach (var image in imageinPost)
					{
						images.Add(image);
					}
				}
			}
			return images;
		}
		public async Task<IEnumerable> GetImageByPostId (string PostId)
        {
            try
            {
                var image = await _context.Images.Where(x => x.PostId == PostId).ToListAsync();
                return image;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting image for post.",ex);
            }
        }
        public async Task<Image> AddAsync(Image entity)
        {
            try
            {
                await _context.Images.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding image", ex);
            }
        }
        public async Task UpdateAsync(Image entity)
        {
            try
            {
                var imagetoUpdate = await _context.Images.AsNoTracking().FirstOrDefaultAsync(x => x.ImageId == entity.ImageId);

                if (imagetoUpdate == null)
                {
                    throw new ArgumentException($"image with id {entity.ImageId} does not exist");
                }

                _context.Images.Update(entity);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating image with id {entity.ImageId}.", ex);
            }
        }

        public async Task DeleteAsync(string key)
        {
			try
			{
				var imageToDelete = await _context.Images.Where(x => x.PostId == key).ToListAsync();
				if (imageToDelete != null)
				{
					for (int i = 0; i < imageToDelete.Count; i++)
						_context.Remove(imageToDelete[i]);
				}
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while deleting image", ex);
			}
		}

        public async Task DeleteImageByPostId(string postId)
        {
            try
            {
                var imagetoDelete = await _context.Images.Where(x => x.PostId == postId).ToListAsync();
                if (imagetoDelete != null)
                {
                    for (int i=0;i<imagetoDelete.Count;i++)
                    {
                        _context.Remove(imagetoDelete[i]); 
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleteting image", ex);
            }
        }
    }
}
