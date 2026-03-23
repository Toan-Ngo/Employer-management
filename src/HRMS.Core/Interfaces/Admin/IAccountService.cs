using HRMS.Core.DTOs;

namespace HRMS.Core.Interfaces.Admin
{
    public interface IAccountService
    {
        Task<List<AccountDto>> GetAccounts(); 
        Task<AccountDto> GetAccount(string id);
        Task<bool> CreateAccount(CreateAccountDto dto); 
        Task<bool> DeleteAccount(string id);
        Task<bool> GeneratePasswordResetCodeAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string code, string newPassword);
    }
}
