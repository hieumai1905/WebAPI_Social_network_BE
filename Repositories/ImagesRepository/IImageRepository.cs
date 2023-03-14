using Web_Social_network_BE.Models;
using System.Collections;
using Web_Social_network_BE.Repositories;

namespace Web_Social_network_BE.Repository.ImageRepository
{
    public interface IImageRepository : IGeneralRepository<Image,string>
    {
        Task<IEnumerable> GetImageById(string key);
    }
}
