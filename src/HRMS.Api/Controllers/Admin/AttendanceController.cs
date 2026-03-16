using HRMS.Core.Entities;
using HRMS.Core.DTOs;
using HRMS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers.AdminApi
{
    [Route("api/admin/attendance")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendances()
        {
            var attendances = await _attendanceService.GetAttendances();
            return Ok(attendances);
        }
        [HttpGet("today")]
        public async Task<IActionResult> GetTodayAttendance()
        {
            var result = await _attendanceService.GetTodayAttendanceAsync();
            return Ok(result);
        }

        [HttpGet("employee/{employeeCode}")]
        public async Task<IActionResult> GetAttendanceByEmployee(string employeeCode)
        {
            var attendances = await _attendanceService
                .GetAttendancesByEmployeeId(employeeCode);

            if (!attendances.Any())
                return NotFound("Không có dữ liệu");

            return Ok(attendances);
        }

        [HttpPost("checkin/{employeeId}")]
        public async Task<IActionResult> CheckIn(int employeeId)
        {
            var result = await _attendanceService.CheckIn(employeeId);

            if (!result)
                return BadRequest("Đã check in hôm nay");

            return Ok("Check in thành công");
        }

        [HttpPut("checkout/{attendanceId}")]
        public async Task<IActionResult> CheckOut(int attendanceId)
        {
            var result = await _attendanceService.CheckOut(attendanceId);

            if (!result)
                return BadRequest("Check out thất bại");

            return Ok("Check out thành công");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAttendanceDto dto)
        {
            var result = await _attendanceService.UpdateAttendance(dto);

            if (!result)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{attendanceId}")]
        public async Task<IActionResult> Delete(int attendanceId)
        {
            var result = await _attendanceService.DeleteAttendance(attendanceId);

            if (!result)
                return BadRequest("Delete thất bại");

            return Ok("Delete thành công");
        }
    }
}
