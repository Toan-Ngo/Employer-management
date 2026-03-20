using HRMS.Core.DTOs;
using HRMS.Core.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers.AdminApi
{
    [Route("api/admin/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)] // Sửa lại thành DTO hiển thị danh sách nhân viên
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeService.GetEmployees();
            return Ok(employees);
        }

        [HttpGet("{EmployeeCode}")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEmployee(string EmployeeCode)
        {
            var employee = await _employeeService.GetEmployee(EmployeeCode);

            if (employee == null)
                return BadRequest(new { message = "Nhân viên không tồn tại" });

            return Ok(employee);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto)
        {
            var employee = await _employeeService.CreateEmployee(dto);

            if (!employee)
                return BadRequest(new { message = "Tạo nhân viên thất bại" });

            return Ok(new { message = "Tạo thành công" });
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeDto dto)
        {
            var success = await _employeeService.UpdateEmployee(dto);

            if (!success)
                return BadRequest(new { message = "Cập nhật không thành công" });

            return Ok(new { message = "Cập nhật thành công" });
        }

        [HttpDelete("{employeeCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteEmployee(string employeeCode)
        {
            var employee = await _employeeService.DeleteEmployee(employeeCode);

            if (!employee)
                return BadRequest(new { message = "Xóa không thành công" });

            return Ok(new { message = "Xóa thành công" });
        }
    }
}