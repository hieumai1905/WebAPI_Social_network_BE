using Web_Social_network_BE.Models;
using System.Collections;
using Web_Social_network_BE.Repositories;

namespace Web_Social_network_BE.Repositories.ImageRepository
{
    public interface IImageRepository : IGeneralRepository<Image,string>
    {
        Task<IEnumerable> GetImageByPostId(string key);
    }
}
