using HRMS.Core.DTOs;
using HRMS.Core.Interfaces.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> CreateAccount(CreateAccountDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.User,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

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
}