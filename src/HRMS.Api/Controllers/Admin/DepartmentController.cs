using HRMS.Core.DTOs;
using HRMS.Core.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers.AdminApi
{
    [Route("api/admin/department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // Lấy danh sách phòng ban
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _departmentService.GetDepartments();
            return Ok(departments);
        }

        // Lấy phòng ban theo tên
        [HttpGet("{departmentName}")]
        public async Task<IActionResult> GetDepartment(string departmentName)
        {
            var department = await _departmentService.GetDepartment(departmentName);

            if (department == null)
                return BadRequest(new { message = "Phòng ban không tồn tại" });

            return Ok(department);
        }

        // Tạo phòng ban
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto dto)
        {
            var department = await _departmentService.CreateDepartment(dto);

            if (department == null)
                return BadRequest(new { message = "Thêm phòng thất bại" });

            return Ok(new { message = "Thêm phòng thành công" });
        }

        // Cập nhật phòng ban
        [HttpPut("{departmentName}")]
        public async Task<IActionResult> UpdateDepartment(string departmentName, [FromBody] DepartmentDto dto)
        {
            var department = await _departmentService.UpdateDepartment(dto);

            if (!department)
                return BadRequest(new { message = "Cập nhật không thành công" });

            return Ok(new { message = "Cập nhật thành công" });
        }

        // Xóa phòng ban
        [HttpDelete("{departmentName}")]
        public async Task<IActionResult> DeleteDepartment(string departmentName)
        {
            var department = await _departmentService.DeleteDepartment(departmentName);

            if (!department)
                return BadRequest(new { message = "Xóa thất bại" });

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
