﻿using Microsoft.AspNetCore.Mvc;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.CommentRepository;
using Web_Social_network_BE.Repositories.ImageRepository;
using Web_Social_network_BE.Repositories.LikeRepository;
using Web_Social_network_BE.Repositories.PostRepository;
using Web_Social_network_BE.Repositories.ReportRepository;

namespace Web_Social_network_BE.Controller
{
    [Route("v2/api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        private readonly IPostRepository _postRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ICommentRepository _commentRepository;


        public ReportController(IReportRepository reportRepository, IPostRepository postRepository, ILikeRepository likeRepository, IImageRepository imageRepository, ICommentRepository commentRepository)
        {
            _reportRepository = reportRepository;
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _imageRepository = imageRepository;
            _commentRepository = commentRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            try
            {
                var report = await _reportRepository.GetAllAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetAllReportsByPostId(string postId)
        {
            try
            {
                var report = await _reportRepository.GetAllReportByPostId(postId);
                if (report == null)
                {
                    return NotFound();
                }
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("{postId}")]
        public async Task<IActionResult> AddReportByPostId(string postId, [FromBody] Report entity)
        {
            try
            {
                var addReport = await _reportRepository.AddReportByPostId(postId, entity);
                return Ok(addReport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding new report: {ex.Message}");
            }
        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePostByPostId(string postId)
        {
            var reportCount = await _reportRepository.GetAllReportByPostId(postId);
            if(reportCount.Count() > 3)
            {
                try
                {
                    await _likeRepository.DeleteAllLikeByPostId(postId);
                    await _commentRepository.DeleteAllCommentsByPostId(postId);
                    await _imageRepository.DeleteImageByPostId(postId);
                    await _postRepository.DeleteAsync(postId);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occurred while deleting post: {ex.Message}");
                }
            }
            return BadRequest();
           
        }
    }
}
