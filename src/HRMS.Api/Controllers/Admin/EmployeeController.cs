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

        // Lấy danh sách nhân viên
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeService.GetEmployees();
            return Ok(employees);
        }

        // Lấy nhân viên theo mã
        [HttpGet("{EmployeeCode}")]
        public async Task<IActionResult> GetEmployee(string EmployeeCode)
        {
            var employee = await _employeeService.GetEmployee(EmployeeCode);

            if (employee == null)
                return BadRequest(new { message = "Nhân viên không tồn tại" });

            return Ok(employee);
        }

        // Tạo nhân viên
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto)
        {
            var employee = await _employeeService.CreateEmployee(dto);

            if (!employee)
                return BadRequest(new { message = "Tạo nhân viên thất bại" });

            return Ok(new { message = "Tạo thành công" });
        }

        // Cập nhật nhân viên
        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(string employeeId, [FromBody] UpdateEmployeeDto dto)
        {
            var employee = await _employeeService.UpdateEmployee(employeeId, dto);

            if (!employee)
                return BadRequest(new { message = "Cập nhật không thành công" });

            return Ok(new { message = "Cập nhật thành công" });
        }

        // Xóa nhân viên
        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(string employeeId)
        {
            var employee = await _employeeService.DeleteEmployee(employeeId);

            if (!employee)
                return BadRequest(new { message = "Xóa không thành công" });

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
