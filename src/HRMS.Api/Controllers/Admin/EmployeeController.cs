using HRMS.Core.DTOs;
using HRMS.Core.Interfaces.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers.AdminApi
{
    [Route("api/admin/employee")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _environment;

        public EmployeeController(IEmployeeService employeeService, IWebHostEnvironment environment)
        {
            _employeeService = employeeService;
            _environment = environment;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)] 
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeService.GetEmployees();
            return Ok(employees);
        }

        [HttpGet("{EmployeeCode}")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployee(string EmployeeCode)
        {
            var employee = await _employeeService.GetEmployee(EmployeeCode);

            if (employee == null)
                return NotFound(new { message = "Nhân viên không tồn tại" });
            var result = new EmployeeDto
            {
                EmployeeCode = employee.EmployeeCode,
                FullName = $"{employee.FirstName} {employee.LastName}",
                DepartmentName = employee.Department?.DepartmentName ?? "Chưa xác định",
                PositionName = employee.Position?.PositionName ?? "Chưa xác định",
                HireDate = employee.HireDate,
                isActive = employee.IsActive,
                Avatar = employee.Avatar
            };

            return Ok(result);
        }
        [HttpPost("{employeeCode}/upload-avatar")]
        public async Task<IActionResult> UploadAvatar(string employeeCode, IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("File không hợp lệ");

            var employee = await _employeeService.GetEmployee(employeeCode);
            if (employee == null) return NotFound("Không tìm thấy nhân viên");

            string rootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            string relativePath = Path.Combine("uploads", "avatars");
            string folderPath = Path.Combine(rootPath, relativePath);

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            if (!string.IsNullOrEmpty(employee.Avatar))
            {
                string oldPath = Path.Combine(rootPath, employee.Avatar.TrimStart('/'));
                if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
            }

            string fileName = $"{employeeCode}_{DateTime.Now.Ticks}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string dbPath = $"/{relativePath.Replace("\\", "/")}/{fileName}";
            await _employeeService.UpdateAvatar(employeeCode, dbPath);

            return Ok(new { url = dbPath });
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