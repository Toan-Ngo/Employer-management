using HRMS.Core.DTOs;
using HRMS.Core.Entities;
using HRMS.Core.Interfaces.Admin;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace HRMS.Data.Services.Admin
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HRMSContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(HRMSContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmployeeDto>> GetEmployees()
        {
            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Select(e => new EmployeeDto
                {
                    EmployeeCode = e.EmployeeCode,
                    FullName = $"{e.FirstName} {e.LastName}",
                    DepartmentName = e.Department != null ? e.Department.DepartmentName : "N/A",
                    PositionName = e.Position != null ? e.Position.PositionName : "N/A",
                    HireDate = e.HireDate,
                    isActive = e.IsActive,
                    Avatar = e.Avatar
                })
                .ToListAsync();
        }

        public async Task<string> GenerateEmployeeCode()
        {
            var lastCode = await _context.Employees
                .AsNoTracking()
                .Select(e => e.EmployeeCode)
                .OrderByDescending(c => c)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(lastCode))
                return "NV001";

            var match = Regex.Match(lastCode, @"\d+");
            if (match.Success)
            {
                int lastNumber = int.Parse(match.Value);
                return "NV" + (lastNumber + 1).ToString("D3");
            }

            return "NV001";
        }

        public async Task<Employee?> GetEmployee(string employeeCode)
        {
            if (string.IsNullOrWhiteSpace(employeeCode)) return null;

            string searchCode = employeeCode.Trim().ToLower();

            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .FirstOrDefaultAsync(e => e.EmployeeCode != null &&
                                          e.EmployeeCode.ToLower() == searchCode);
        }

        public async Task<bool> CreateEmployee(CreateEmployeeDto dto)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentName.ToLower() == dto.DepartmentName.ToLower().Trim());
            if (department == null) return false;

            var position = await _context.Positions
                .FirstOrDefaultAsync(p => p.PositionName.ToLower() == dto.PositionName.ToLower().Trim());
            if (position == null) return false;

            var employee = new Employee
            {
                EmployeeCode = await GenerateEmployeeCode(),
                FirstName = dto.FirstName?.Trim() ?? "",
                LastName = dto.LastName?.Trim() ?? "",
                Email = dto.Email?.Trim() ?? "",
                Phone = dto.Phone?.Trim() ?? "",
                DateOfBirth = dto.DateOfBirth,
                Gender = (Employee.GenderType)dto.Gender,
                Address = dto.Address,
                DepartmentId = department.Id,
                PositionId = position.Id,
                HireDate = DateTime.Now,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            await _context.Employees.AddAsync(employee);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateEmployee(UpdateEmployeeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.EmployeeCode)) return false;

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeCode == dto.EmployeeCode.Trim());

            if (employee == null) return false;

            if (!string.IsNullOrEmpty(dto.DepartmentName))
            {
                var department = await _context.Departments
                    .FirstOrDefaultAsync(d => d.DepartmentName.ToLower() == dto.DepartmentName.ToLower().Trim());
                if (department != null) employee.DepartmentId = department.Id;
            }

            if (!string.IsNullOrEmpty(dto.PositionName))
            {
                var position = await _context.Positions
                    .FirstOrDefaultAsync(p => p.PositionName.ToLower() == dto.PositionName.ToLower().Trim());
                if (position != null) employee.PositionId = position.Id;
            }

            employee.UpdatedAt = DateTime.Now;
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteEmployee(string employeeCode)
        {
            if (string.IsNullOrWhiteSpace(employeeCode)) return false;

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode.Trim());

            if (employee == null) return false;

            employee.IsActive = false;
            employee.UpdatedAt = DateTime.Now;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateAvatar(string employeeCode, string avatarPath)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);

            if (employee == null) return false;

            employee.Avatar = avatarPath; 
            employee.UpdatedAt = DateTime.Now;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}