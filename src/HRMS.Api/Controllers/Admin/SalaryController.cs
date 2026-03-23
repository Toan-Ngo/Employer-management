using HRMS.Core.DTOs;
using HRMS.Core.Interfaces.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers.AdminApi
{
    [Route("api/admin/salary")]
    [ApiController]
    [Authorize]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryService _salaryService;

        public SalaryController(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SalaryDto>), StatusCodes.Status200OK)] // Thay bằng DTO của bạn
        public async Task<IActionResult> GetSalaries()
        {
            var salaries = await _salaryService.GetSalaries();
            return Ok(salaries);
        }

        [HttpGet("employee/{employeeCode}")]
        [ProducesResponseType(typeof(IEnumerable<SalaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSalary(string employeeCode)
        {
            var salary = await _salaryService.GetSalary(employeeCode);

            if (salary == null || !salary.Any())
                return BadRequest("Không tìm thấy lương nhân viên");

            return Ok(salary);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSalary(CreateSalaryDto dto)
        {
            var result = await _salaryService.CreateSalary(dto);

            if (!result)
                return BadRequest("Tạo lương thất bại");

            return Ok("Tạo lương thành công");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSalary(int id, UpdateSalaryDto dto)
        {
            var result = await _salaryService.UpdateSalary(id, dto);

            if (!result)
                return BadRequest("Cập nhật lương thất bại");

            return Ok("Cập nhật thành công");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteSalary(int id)
        {
            var result = await _salaryService.DeleteSalary(id);

            if (!result)
                return BadRequest("Xóa lương thất bại");

            return Ok("Xóa thành công");
        }
    }
}