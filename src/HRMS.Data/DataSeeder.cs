using HRMS.Core.Entities;
using HRMS.Core.RBAC;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace HRMS.Data
{
    public class DataSeeder
    {
        private readonly HRMSContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeder(
            HRMSContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            // Department
            if (!await _context.Departments.AnyAsync())
            {
                await _context.Departments.AddAsync(new Department { DepartmentName = "Administration" });
                await _context.SaveChangesAsync();
            }

            // Position
            if (!await _context.Positions.AnyAsync())
            {
                await _context.Positions.AddAsync(new Position { PositionName = "Administrator" });
                await _context.SaveChangesAsync();
            }

            // Employee
            if (!await _context.Employees.AnyAsync())
            {
                var department = await _context.Departments.FirstAsync();
                var position = await _context.Positions.FirstAsync();

                await _context.Employees.AddAsync(new Employee
                {
                    EmployeeCode = "EMP001",
                    FirstName = "Toàn",
                    LastName = "Trọng",
                    Email = "rootadmin@gmail.com",
                    Phone = "0123456789",
                    Gender = Employee.GenderType.Male,
                    DepartmentId = department.Id,
                    PositionId = position.Id,
                    HireDate = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                });

                await _context.SaveChangesAsync();
            }
            //  Role
            string[] roles = { Roles.Admin, Roles.HR, Roles.Employee };
            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // User
            var employeeEntity = await _context.Employees.FirstAsync();
            var user = await _userManager.FindByNameAsync("rootadmin");
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "rootadmin",
                    Email = "rootadmin@gmail.com",
                    EmployeeId = employeeEntity.Id,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    RefreshToken = ""
                };

                var result = await _userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    // Gán role Admin
                    await _userManager.AddToRoleAsync(user, Roles.Admin);
                }
            }
        }
    }
}