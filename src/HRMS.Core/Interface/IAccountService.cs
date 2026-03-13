namespace HRMS.Core.Interface
{
    public interface IAccountService
    {
        Task<List<ApplicationUser>> GetAllAccounts(); // lấy tất cả tài khoản
        Task<List<ApplicationUser>> GetAccount(string id); // lấy tài khoản theo id
        Task<bool> CreateAccount(ApplicationUser user, string password); // tạo tài khoản mới
        Task<bool> DeleteAccount(string id); // xóa tài khoản theo id

    }
}
