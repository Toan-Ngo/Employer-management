using HRMS.Core.DTOs;
using HRMS.Core.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers.AdminApi
{
    [Route("api/admin/position")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PositionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPositions()
        {
            var positions = await _positionService.GetPositions();
            return Ok(positions);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePosition([FromBody] PositionDto dto)
        {
            var result = await _positionService.CreatePosition(dto);
            if (!result) return BadRequest("Tạo chức vụ thất bại (có thể tên đã tồn tại)");
            return Ok(new { message = "Tạo chức vụ thành công" });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePosition(int id, PositionDto dto)
        {
            var result = await _positionService.UpdatePosition(id, dto.PositionName);
            if (!result) return BadRequest("Cập nhật thất bại");
            return Ok(new { message = "Cập nhật thành công" });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var result = await _positionService.DeletePosition(id);
            if (!result) return BadRequest("Xóa thất bại (Chức vụ đang có nhân viên hoặc không tồn tại)");
            return Ok(new { message = "Xóa thành công" });
        }
    }
}