using HRMS.Core.Interfaces.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers.AdminApi
{
    [Route("api/admin/department")]
    [Authorize]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DepartmentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _departmentService.GetDepartments();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var department = await _departmentService.GetDepartment(id);

            if (department == null)
                return BadRequest(new { message = "Phòng ban không tồn tại" });

            return Ok(department);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto dto)
        {
            var department = await _departmentService.CreateDepartment(dto);

            if (department == null)
                return BadRequest(new { message = "Thêm phòng thất bại" });

            return Ok(new { message = "Thêm phòng thành công" });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentDto departmentDto)
        {
            // Đảm bảo truyền đúng giá trị vào Service
            var success = await _departmentService.UpdateDepartment(id, departmentDto.DepartmentName);

            if (!success)
                return BadRequest(new { message = "Cập nhật thất bại" });

            return Ok(new { message = "Cập nhật thành công" });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var success = await _departmentService.DeleteDepartment(id);

            if (!success)
                return BadRequest(new { message = "Không thể xóa phòng ban đang có nhân viên" });

            return Ok(new { message = "Xóa phòng ban thành công" });
        }
    }
}