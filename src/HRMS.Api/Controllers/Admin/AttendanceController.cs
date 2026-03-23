using HRMS.Core.DTOs;
using HRMS.Core.Entities;
using HRMS.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers.AdminApi
{
    [Route("api/admin/attendance")]
    [ApiController]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AttendanceDto>), StatusCodes.Status200OK)] // Cần có DTO tương ứng
        public async Task<IActionResult> GetAttendances()
        {
            var attendances = await _attendanceService.GetAttendances();
            return Ok(attendances);
        }

        [HttpGet("today")]
        [ProducesResponseType(typeof(IEnumerable<AttendanceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTodayAttendance()
        {
            var result = await _attendanceService.GetTodayAttendanceAsync();
            return Ok(result);
        }

        [HttpGet("employee/{employeeCode}")]
        [ProducesResponseType(typeof(IEnumerable<AttendanceDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAttendanceByEmployee(string employeeCode)
        {
            var attendances = await _attendanceService.GetAttendancesByEmployeeId(employeeCode);

            if (!attendances.Any())
                return NotFound("Không có dữ liệu");

            return Ok(attendances);
        }

        [HttpPost("checkin/{employeeId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckIn(int employeeId)
        {
            var result = await _attendanceService.CheckIn(employeeId);

            if (!result)
                return BadRequest("Đã check in hôm nay");

            return Ok("Check in thành công");
        }

        [HttpPut("checkout/{attendanceId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckOut(int attendanceId)
        {
            var result = await _attendanceService.CheckOut(attendanceId);

            if (!result)
                return BadRequest("Check out thất bại");

            return Ok("Check out thành công");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UpdateAttendanceDto dto)
        {
            var result = await _attendanceService.UpdateAttendance(dto);

            if (!result)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{attendanceId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int attendanceId)
        {
            var result = await _attendanceService.DeleteAttendance(attendanceId);

            if (!result)
                return BadRequest("Delete thất bại");

            return Ok("Delete thành công");
        }
    }
}