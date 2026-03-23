using HRMS.Core.DTOs;
using HRMS.Core.Interfaces.Admin;
using HRMS.Core.Interfaces.Auth;

// Nhớ using namespace chứa IEmailService của bạn vào đây nhé
using HRMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly HRMSContext _context;
    private readonly IEmailService _emailService; // Thêm IEmailService

    // Inject đầy đủ các Service cần thiết
    public AccountService(
        UserManager<ApplicationUser> userManager,
        HRMSContext context,
        IEmailService emailService)
    {
        _userManager = userManager;
        _context = context;
        _emailService = emailService;
    }

    public async Task<bool> CreateAccount(CreateAccountDto dto)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == dto.Email);

        if (employee == null)
        {
            return false;
        }

        var user = new ApplicationUser
        {
            UserName = dto.User,
            Email = dto.Email,
            EmployeeId = employee.Id,
            CreatedAt = DateTime.Now,
            IsActive = true,
            RefreshToken = ""
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded && !string.IsNullOrEmpty(dto.Role))
        {
            await _userManager.AddToRoleAsync(user, dto.Role);
        }

        return result.Succeeded;
    }

    public async Task<bool> DeleteAccount(string employeeCode)
    {
        var user = await _userManager.Users
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Employee.EmployeeCode == employeeCode);

        if (user == null)
            return false;

        var result = await _userManager.DeleteAsync(user);

        return result.Succeeded;
    }

    public async Task<AccountDto?> GetAccount(string employeeCode)
    {
        var user = await _userManager.Users
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Employee.EmployeeCode == employeeCode);

        if (user == null) return null;

        return new AccountDto
        {
            Email = user.Email,
            User = user.UserName
        };
    }

    public async Task<List<AccountDto>> GetAccounts()
    {
        return await _userManager.Users
            .Select(u => new AccountDto
            {
                Email = u.Email,
                User = u.UserName
            })
            .ToListAsync();
    }

    public async Task<bool> GeneratePasswordResetCodeAsync(string email)
    {

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var random = new Random();
        string code = random.Next(100000, 999999).ToString();

        user.ResetCode = code;
        user.ResetCodeExpiry = DateTime.UtcNow.AddMinutes(15);

        await _userManager.UpdateAsync(user);

        string emailBody = $"<h3>Yêu cầu đặt lại mật khẩu</h3><p>Mã xác thực của bạn là: <b>{code}</b></p><p>Mã này sẽ hết hạn trong vòng 15 phút.</p>";
        await _emailService.SendEmailAsync(email, "Mã xác thực đổi mật khẩu - HRMS", emailBody);

        return true;
    }

    public async Task<bool> ResetPasswordAsync(string email, string code, string newPassword)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email && u.ResetCode == code);

        if (user == null || user.ResetCodeExpiry < DateTime.UtcNow)
            return false;

        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, newPassword);

        user.ResetCode = null;
        user.ResetCodeExpiry = null;

        await _userManager.UpdateAsync(user);
        return true;
    }
}