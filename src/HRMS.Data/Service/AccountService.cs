using HRMS.Core.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        // Tạo tài khoản mới
        public async Task<bool> CreateAccount(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }
        // Xóa tài khoản
        public async Task<bool> DeleteAccount(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Employee.EmployeeCode == id);
            if (user == null)
            {
                return false; // Không tìm thấy người dùng
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        // Cập nhật thông tin tài khoản
        public async Task<ApplicationUser> GetAccount(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Employee.EmployeeCode == id);
            if (user == null)
            {
                return null; // Không tìm thấy người dùng
            }
            return user;
        }
        // Lấy danh sách tất cả tài khoản
        public async Task<List<ApplicationUser>> GetAllAccounts()
        {
            return _userManager.Users.ToList();
        }
    }
}
