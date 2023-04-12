using System.Collections;
using Web_Social_network_BE.Models;

namespace Web_Social_network_BE.Repositories.PostRepository
{
    public interface IPostRepository : IGeneralRepository<Post, string>
    {
        //Lấy hết bài đăng của người dùng có id là
        Task<IEnumerable<Post>> GetAllAsyncByUserId(string userId);
        //Lấy hết những người dùng đã like bài
        Task<IEnumerable<Like>> GetAllUserLikeAsync(string postId);

        Task<IEnumerable<Post>> GetAllInMonthAsync();

	Task<IEnumerable<Post>> GetAllPostForHomeAsync(string userId);
	}
}
