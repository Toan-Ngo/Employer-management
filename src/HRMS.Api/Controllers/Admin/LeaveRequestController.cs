using HRMS.Core.DTOs; // Thêm dòng này để dùng các DTO nếu cần
using HRMS.Core.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers.AdminApi
{
    [Route("api/admin/leaverequest")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LeaveRequestDto>), StatusCodes.Status200OK)] // Giả sử dùng LeaveRequestDto
        public async Task<IActionResult> GetLeaveRequests()
        {
            var result = await _leaveRequestService.GetLeaveRequests();
            return Ok(result);
        }

        [HttpGet("employee/{employeeCode}")]
        [ProducesResponseType(typeof(IEnumerable<LeaveRequestDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLeaveRequestsByEmployee(string employeeCode)
        {
            var result = await _leaveRequestService.GetLeaveRequestByEmployee(employeeCode);
            return Ok(result);
        }

        [HttpPut("approve/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApproveLeaveRequest(int id)
        {
            var result = await _leaveRequestService.ApproveLeaveRequest(id);

            if (!result)
                return BadRequest("Duyệt đơn thất bại");

            return Ok("Đã duyệt đơn nghỉ");
        }

        [HttpPut("reject/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RejectLeaveRequest(int id)
        {
            var result = await _leaveRequestService.RejectLeaveRequest(id);

            if (!result)
                return BadRequest("Từ chối đơn thất bại");

            return Ok("Đã từ chối đơn nghỉ");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteLeaveRequest(int id)
        {
            var result = await _leaveRequestService.DeleteLeaveRequest(id);

            if (!result)
                return BadRequest("Xóa không thành công");

            return Ok("Xóa thành công");
        }
    }
}