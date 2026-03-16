using HRMS.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                await _context.Departments.AddAsync(new Department
                {
                    DepartmentName = "Administration"
                });

                await _context.SaveChangesAsync();
            }

            // Position
            if (!await _context.Positions.AnyAsync())
            {
                await _context.Positions.AddAsync(new Position
                {
                    PositionName = "Administrator"
                });

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
                    FirstName = "Root",
                    LastName = "Admin",
                    Email = "admin@gmail.com",
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
            string[] roles = { "RootAdmin", "HR", "Manager" }; 
            foreach (var role in roles) { 
                if (!await _roleManager.RoleExistsAsync(role)) 
                { 
                    await _roleManager.CreateAsync(new IdentityRole(role)); 
                } 
            }

            var employee = await _context.Employees.FirstAsync();

            
            

            // User
            var user = await _userManager.FindByNameAsync("rootadmin");

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "rootadmin",
                    Email = "admin@gmail.com",
                    EmployeeId = employee.Id,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    RefreshToken = ""
                };

                var result = await _userManager.CreateAsync(user, "Admin@123");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "RootAdmin");
                }
            }
        }
    }
}