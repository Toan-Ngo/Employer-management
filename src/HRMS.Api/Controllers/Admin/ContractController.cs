using HRMS.Core.DTOs;
using HRMS.Core.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers.AdminApi
{
    [Route("api/admin/contract")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        // Lấy danh sách hợp đồng
        [HttpGet]
        public async Task<IActionResult> GetContracts()
        {
            var contracts = await _contractService.GetContracts();
            return Ok(contracts);
        }

        // Lấy hợp đồng theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContract(int id)
        {
            var contract = await _contractService.GetContractById(id);

            if (contract == null)
                return NotFound(new { message = "Contract not found" });

            return Ok(contract);
        }

        // Tạo hợp đồng
        [HttpPost]
        public async Task<IActionResult> CreateContract([FromBody] CreateContractDto dto)
        {
            var newContract = await _contractService.CreateContract(dto);

            if (!newContract)
                return BadRequest(new { message = "Tạo hợp đồng thất bại" });

            return Ok(new { message = "Tạo hợp đồng thành công" });
        }

        // Gia hạn / cập nhật hợp đồng
        [HttpPut("{contractId}/extend")]
        public async Task<IActionResult> UpdateContract(int contractId, [FromBody] UpdateContractDto dto)
        {
            var contract = await _contractService.UpdateContract(contractId, dto);

            if (!contract)
                return BadRequest(new { message = "Cập nhật hợp đồng không thành công" });

            return Ok(new { message = "Cập nhật hợp đồng thành công" });
        }

        // Xóa hợp đồng
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            var deleteContract = await _contractService.DeleteContract(id);

            if (!deleteContract)
                return BadRequest(new { message = "Xóa thất bại" });

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
