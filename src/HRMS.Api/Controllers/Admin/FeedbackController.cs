using HRMS.Core.DTOs;
using HRMS.Core.Interfaces.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMS.Api.Controllers.AdminApi
{
    [Route("api/admin/feedback")]
    [ApiController]
    [Authorize] // Yêu cầu đăng nhập
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,HR")]
        [ProducesResponseType(typeof(IEnumerable<FeedbackDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
            return Ok(feedbacks);
        }

        [HttpGet("my-feedbacks")]
        [ProducesResponseType(typeof(IEnumerable<FeedbackDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyFeedbacks()
        {
            // Lấy EmployeeCode từ Identity Claims
            var empCode = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;

            if (string.IsNullOrEmpty(empCode))
                return Unauthorized("Không xác định được danh tính nhân viên.");

            var feedbacks = await _feedbackService.GetFeedbacksByEmployeeAsync(empCode);
            return Ok(feedbacks);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateFeedbackDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var empCode = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;

            var result = await _feedbackService.CreateFeedbackAsync(empCode, dto);

            if (result)
                return Ok(new { message = "Gửi phản hồi thành công!" });

            return BadRequest("Lỗi khi lưu phản hồi vào hệ thống.");
        }

        
        [HttpPut("{id}/reply")]
        [Authorize(Roles = "Admin,HR")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Reply(int id, [FromBody] AdminReplyDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _feedbackService.ReplyFeedbackAsync(id, dto);

            if (result)
                return Ok(new { message = "Phản hồi đã được gửi đi thành công." });

            return NotFound(new { message = $"Không tìm thấy bản ghi phản hồi ID: {id}" });
        }

        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _feedbackService.DeleteFeedbackAsync(id);
            if (!result) return BadRequest("Xóa thất bại");
            return Ok("Xóa thành công");
        }
    }
}