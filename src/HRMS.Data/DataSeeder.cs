using HRMS.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Data
{
    public class DataSeeder
    {
        public async Task SeedAsync(HRMSContext context)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            // Department
            if (!context.Departments.Any())
            {
                await context.Departments.AddAsync(new Department
                {
                    DepartmentName = "Administration"
                });
                await context.SaveChangesAsync();
            }

            // Position
            if (!context.Positions.Any())
            {
                await context.Positions.AddAsync(new Position
                {
                    PositionName = "Administrator"
                });
                await context.SaveChangesAsync();
            }

            // Employee
            if (!context.Employees.Any())
            {
                var department = context.Departments.First();
                var position = context.Positions.First();

                await context.Employees.AddAsync(new Employee
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

                await context.SaveChangesAsync();
            }

            var employee = context.Employees.First();

            // Role
            if (!context.Roles.Any())
            {
                await context.Roles.AddAsync(new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "RootAdmin",
                    NormalizedName = "ROOTADMIN"
                });

                await context.SaveChangesAsync();
            }

            var role = context.Roles.First();

            // User
            if (!context.Users.Any())
            {
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "rootadmin",
                    NormalizedUserName = "ROOTADMIN",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmployeeId = employee.Id,
                    CreatedAt = DateTime.Now,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = false
                };

                user.PasswordHash = passwordHasher.HashPassword(user, "Admin@123");

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                await context.UserRoles.AddAsync(new IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
