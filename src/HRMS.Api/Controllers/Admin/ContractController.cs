using HRMS.Core.DTOs;
using HRMS.Core.Interfaces.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers.AdminApi
{
    [Route("api/admin/contract")]
    [ApiController]
    [Authorize]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContractDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContracts()
        {
            var contracts = await _contractService.GetContracts();
            return Ok(contracts);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContractDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContract(int id)
        {
            var contract = await _contractService.GetContractById(id);
            if (contract == null) return NotFound(new { message = "Không tìm thấy hợp đồng" });
            return Ok(contract);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContract([FromBody] CreateContractDto dto)
        {
            var success = await _contractService.CreateContract(dto);
            if (!success) return BadRequest(new { message = "Lỗi khi tạo hợp đồng (Nhân viên không tồn tại)" });
            return Ok(new { message = "Tạo hợp đồng thành công" });
        }

        [HttpPut("{contractId}/extend")]
        public async Task<IActionResult> UpdateContract(int contractId, [FromBody] UpdateContractDto dto)
        {
            var success = await _contractService.UpdateContract(contractId, dto);
            if (!success) return BadRequest(new { message = "Cập nhật thất bại" });
            return Ok(new { message = "Gia hạn hợp đồng thành công" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            var success = await _contractService.DeleteContract(id);
            if (!success) return BadRequest(new { message = "Xóa thất bại" });
            return Ok(new { message = "Xóa thành công" });
        }
    }
}