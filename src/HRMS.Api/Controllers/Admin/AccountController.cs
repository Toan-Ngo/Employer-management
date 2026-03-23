using HRMS.Core.DTOs;
using HRMS.Core.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Api.Controllers.AdminApi
{
    [Route("api/admin/account")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // Lấy danh sách tất cả tài khoản
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AccountDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccounts()
        {
            var accounts = await _accountService.GetAccounts();
            return Ok(accounts);
        }

        // Lấy tài khoản theo ID
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccount(string id)
        {
            var account = await _accountService.GetAccount(id);

            if (account == null)
                return NotFound(new { message = "Account not found" });

            return Ok(account);
        }

        // Tạo tài khoản mới
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
        {
            var result = await _accountService.CreateAccount(dto);

            if (result)
                return Ok(new { message = "Account created successfully" });

            return BadRequest(new { message = "Failed to create account" });
        }

        // Xóa tài khoản
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var result = await _accountService.DeleteAccount(id);

            if (result)
                return Ok(new { message = "Account deleted successfully" });

            return BadRequest(new { message = "Failed to delete account" });
        }
    }
}
