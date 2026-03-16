using HRMS.Core.DTOs;

namespace HRMS.Core.Interfaces.Admin
{
    public interface IAccountService
    {
        Task<List<AccountDto>> GetAccounts(); // lấy tất cả tài khoản
        Task<AccountDto> GetAccount(string id); // lấy tài khoản theo id
        Task<bool> CreateAccount(CreateAccountDto dto); // tạo tài khoản mới
        Task<bool> DeleteAccount(string id); // xóa tài khoản theo id
    }
}
