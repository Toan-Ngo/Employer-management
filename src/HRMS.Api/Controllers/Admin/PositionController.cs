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

        // Lấy tất cả chức vụ
        [HttpGet]
        public async Task<IActionResult> GetPositions()
        {
            var positions = await _positionService.GetPositions();
            return Ok(positions);
        }

        // Lấy 1 chức vụ theo tên
        [HttpGet("{positionName}")]
        public async Task<IActionResult> GetPosition(string positionName)
        {
            var position = await _positionService.GetPosition(positionName);

            if (position == null)
                return BadRequest("Chức vụ không tồn tại");

            return Ok(position);
        }

        // Tạo chức vụ
        [HttpPost]
        public async Task<IActionResult> CreatePosition(PositionDto dto)
        {
            var result = await _positionService.CreatePosition(dto);

            if (!result)
                return BadRequest("Tạo chức vụ thất bại");

            return Ok("Tạo chức vụ thành công");
        }

        // Cập nhật chức vụ
        [HttpPut]
        public async Task<IActionResult> UpdatePosition(PositionDto dto)
        {
            var result = await _positionService.UpdatePosition(dto);

            if (!result)
                return BadRequest("Cập nhật thất bại");

            return Ok("Cập nhật thành công");
        }

        // Xóa chức vụ
        [HttpDelete("{positionName}")]
        public async Task<IActionResult> DeletePosition(string positionName)
        {
            var result = await _positionService.DeletePosition(positionName);

            if (!result)
                return BadRequest("Xóa thất bại");

            return Ok("Xóa thành công");
        }
    }
}