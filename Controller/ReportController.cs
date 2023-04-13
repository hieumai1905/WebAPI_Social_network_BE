using Microsoft.AspNetCore.Mvc;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.PostRepository;
using Web_Social_network_BE.Repositories.ReportRepository;

namespace Web_Social_network_BE.Controller
{
    [Route("v2/api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
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
    }
}
