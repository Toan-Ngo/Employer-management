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

        // Lấy tất cả đơn nghỉ
        [HttpGet]
        public async Task<IActionResult> GetLeaveRequests()
        {
            var result = await _leaveRequestService.GetLeaveRequests();
            return Ok(result);
        }

        // Lấy đơn nghỉ theo nhân viên
        [HttpGet("employee/{employeeCode}")]
        public async Task<IActionResult> GetLeaveRequestsByEmployee(string employeeCode)
        {
            var result = await _leaveRequestService.GetLeaveRequestByEmployee(employeeCode);
            return Ok(result);
        }

        // Duyệt đơn nghỉ
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveLeaveRequest(int id)
        {
            var result = await _leaveRequestService.ApproveLeaveRequest(id);

            if (!result)
                return BadRequest("Duyệt đơn thất bại");

            return Ok("Đã duyệt đơn nghỉ");
        }

        // Từ chối đơn nghỉ
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectLeaveRequest(int id)
        {
            var result = await _leaveRequestService.RejectLeaveRequest(id);

            if (!result)
                return BadRequest("Từ chối đơn thất bại");

            return Ok("Đã từ chối đơn nghỉ");
        }

        // Xóa đơn nghỉ
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveRequest(int id)
        {
            var result = await _leaveRequestService.DeleteLeaveRequest(id);

            if (!result)
                return BadRequest("Xóa không thành công");

            return Ok("Xóa thành công");
        }
    }
}