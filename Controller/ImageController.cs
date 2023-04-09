using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.ImageRepository;

namespace Web_Social_network_BE.Controller
{
    [Route("vi/api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository; 
        public ImageController (IImageRepository imageRepository)
        {
            _imageRepository = imageRepository; 
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var image = await _imageRepository.GetAllAsync();
                return Ok(image);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting all image: {ex.Message}");
            }
        }
        //Lấy ra ảnh có id = ImageId
        [HttpGet("{ImageId}/Image")] 
        public async Task<IActionResult> GetImageById (string ImageId)
        {
            try
            {
                var image = await _imageRepository.GetByIdAsync(ImageId);
                return Ok(image);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting image: {ex.Message}");
            }
        }
        //Lấy ra toàn bộ ảnh trong post có id = PostId
        [HttpGet("{PostId}")]
        public async Task<IActionResult> GetImageByPostId(string PostId)
        {
            try
            {
                var image = await _imageRepository.GetImageByPostId(PostId);
                return Ok(image);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while getting image of 1 post: {ex.Message}");
            }
        }
        //Lấy ra toàn bộ ảnh của user
        [HttpGet("{userId}/Images")]
		public async Task<IActionResult> GetImageByUserId(string userId)
		{
			try
			{
				var images = await _imageRepository.GetImageByUserId(userId);
				return Ok(images);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while getting image by user id ");
			}
		}
		//Thêm ảnh
		[HttpPost]
        public async Task<IActionResult> Add([FromBody] Image image)
        {
            try
            {
                image.ImageId = Guid.NewGuid().ToString();
                var addedImage = await _imageRepository.AddAsync(image);
                return CreatedAtAction(nameof(GetImageById), new { ImageId = addedImage.ImageId }, addedImage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding image for post: {ex.Message}");
            }
        }
        //Sửa ảnh có id = ImageId
        [HttpPut("{ImageId}")]
        public async Task<IActionResult> Update(string ImageId, [FromBody] Image image)
        {
            if (ImageId != image.ImageId)
            {
                return BadRequest("Image in the request body does not match the id in the URL");
            }

            await _imageRepository.UpdateAsync(image);
            return Ok();
        }
		//Xóa ảnh có id = ImageId
		[HttpDelete("{PostId}")]
		public async Task<IActionResult> Delete(string PostId)
		{
			try
			{
				await _imageRepository.DeleteAsync(PostId);
				return NoContent();
			}
			catch (ArgumentException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while deleting Image with id : {ex.Message}");
			}
		}
	}
}
