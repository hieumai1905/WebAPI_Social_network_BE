using Web_Social_network_BE.Models;
using System.Collections;
using Web_Social_network_BE.Repositories;

namespace Web_Social_network_BE.Repositories.ImageRepository
{
    public interface IImageRepository : IGeneralRepository<Image,string>
    {
		Task<IEnumerable> GetImageByUserId(string userId);
		Task<IEnumerable> GetImageByPostId(string PostId);
        Task DeleteImageByPostId(string postId);
    }
}
