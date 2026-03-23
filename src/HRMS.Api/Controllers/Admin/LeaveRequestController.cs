using HRMS.Core.DTOs;
using HRMS.Core.Interfaces.Admin;
using HRMS.Core.RBAC; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers.AdminApi
{
    [Route("api/admin/leaverequest")]
    [ApiController]
    [Authorize]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        #region ADMIN & HR ENDPOINTS (Quản lý)

        [HttpGet]
        [Authorize(Roles = Roles.Admin + "," + Roles.HR)]
        [ProducesResponseType(typeof(List<LeaveRequestDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLeaveRequests()
        {
            var result = await _leaveRequestService.GetLeaveRequests();
            return Ok(result);
        }

        [HttpPut("approve/{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.HR)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApproveLeaveRequest(int id)
        {
            var result = await _leaveRequestService.ApproveLeaveRequest(id);
            if (!result) return BadRequest("Duyệt đơn thất bại hoặc đơn không tồn tại.");

            return Ok("Đã duyệt đơn nghỉ thành công.");
        }

        [HttpPut("reject/{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.HR)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RejectLeaveRequest(int id)
        {
            var result = await _leaveRequestService.RejectLeaveRequest(id);
            if (!result) return BadRequest("Từ chối đơn thất bại hoặc đơn không tồn tại.");

            return Ok("Đã từ chối đơn nghỉ thành công.");
        }

        #endregion

        #region EMPLOYEE PORTAL ENDPOINTS (Nhân viên)

        [HttpGet("employee/{employeeCode}")]
        [ProducesResponseType(typeof(List<LeaveRequestDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLeaveRequestsByEmployee(string employeeCode)
        {
            var result = await _leaveRequestService.GetLeaveRequestByEmployee(employeeCode);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] CreateLeaveRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _leaveRequestService.CreateLeaveRequest(request);
            if (!result) return BadRequest("Không thể tạo đơn nghỉ phép. Vui lòng kiểm tra lại mã nhân viên.");

            return Ok("Gửi đơn nghỉ phép thành công.");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateLeaveRequest(int id, [FromBody] UpdateLeaveRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _leaveRequestService.UpdateLeaveRequest(id, request);
            if (!result) return BadRequest("Cập nhật đơn thất bại. Lưu ý: Chỉ có thể sửa đơn khi đang ở trạng thái 'Chờ duyệt'.");

            return Ok("Cập nhật đơn nghỉ thành công.");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteLeaveRequest(int id)
        {
            var result = await _leaveRequestService.DeleteLeaveRequest(id);
            if (!result) return BadRequest("Xóa thất bại. Đơn không tồn tại hoặc đã được xử lý (không còn ở trạng thái Chờ duyệt).");

            return Ok("Xóa đơn thành công.");
        }

        #endregion
    }
}