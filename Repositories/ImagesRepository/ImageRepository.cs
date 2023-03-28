using Web_Social_network_BE.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Web_Social_network_BE.Repositories.ImageRepository
{
    public class ImageRepository : IImageRepository
    {
        private readonly SocialNetworkN01Context _context;
        public ImageRepository(SocialNetworkN01Context context)
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
                var imagetoDelete = await _context.Images.FindAsync(key);

                if (imagetoDelete == null)
                {
                    throw new ArgumentException("Image does not exist");
                }
                _context.Images.Remove(imagetoDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting image", ex);
            }
        }
    }
}
