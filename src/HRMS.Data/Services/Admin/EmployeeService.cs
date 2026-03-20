using HRMS.Core.DTOs;
using HRMS.Core.Entities;
using HRMS.Core.Interfaces.Admin;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data.Services.Admin
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HRMSContext _context; // sử dụng HRMSContext để tương tác với cơ sở dữ liệu
        private readonly IUnitOfWork _unitOfWork; // sử dụng IUnitOfWork để quản lý giao dịch
        public EmployeeService(HRMSContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<EmployeeDto>> GetEmployees()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Select(e => new EmployeeDto
                {
                    EmployeeCode = e.EmployeeCode,
                    FullName = e.FirstName + " " + e.LastName,
                    DepartmentName = e.Department.DepartmentName,
                    PositionName = e.Position.PositionName,
                    HireDate = e.HireDate,
                    isActive = e.IsActive

                })
                .ToListAsync();
        }
        public async Task<string> GenerateEmployeeCode()
        {
            var lastEmployee = await _context.Employees
                .OrderByDescending(e => e.Id)
                .FirstOrDefaultAsync();

            if (lastEmployee == null)
                return "NV001";

            var lastNumber = int.Parse(lastEmployee.EmployeeCode.Substring(2));
            return "NV" + (lastNumber + 1).ToString("D3");
        }
        public async Task<Employee> GetEmployee(string EmployeeCode)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .FirstOrDefaultAsync(e => e.EmployeeCode.ToLower().Trim() == EmployeeCode.ToLower().Trim());
            if (employee == null)
            {
                return null;
            }
            return employee;
        }
        public async Task<bool> CreateEmployee(CreateEmployeeDto dto)
        {
            // Tìm phòng ban
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentName.Trim().ToLower() == dto.DepartmentName.Trim().ToLower());
            if (department == null) return false;

            // Tìm chức vụ
            var position = await _context.Positions
                .FirstOrDefaultAsync(p => p.PositionName.Trim().ToLower() == dto.PositionName.Trim().ToLower());
            if (position == null) return false;

            var employee = new Employee
            {
                EmployeeCode = await GenerateEmployeeCode(),
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = dto.Email.Trim(),
                Phone = dto.Phone.Trim(),
                DateOfBirth = dto.DateOfBirth,
                Gender = (Employee.GenderType)dto.Gender,
                Address = dto.Address,
                DepartmentId = department.Id,
                PositionId = position.Id,
                HireDate = DateTime.Now,
                IsActive = true,
                CreatedAt = DateTime.Now // Đảm bảo gán ngày tạo để tránh lỗi DB
            };

            await _context.Employees.AddAsync(employee);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateEmployee(UpdateEmployeeDto dto)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeCode.Trim() == dto.EmployeeCode.Trim());

            if (employee == null) return false;

            // Tìm phòng ban
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentName.ToLower().Trim() == dto.DepartmentName.ToLower().Trim());

            if (department != null) employee.DepartmentId = department.Id;

            // Tìm chức vụ 
            if (!string.IsNullOrEmpty(dto.PositionName))
            {
                var position = await _context.Positions
                    .FirstOrDefaultAsync(p => p.PositionName.ToLower().Trim() == dto.PositionName.ToLower().Trim());
                if (position != null) employee.PositionId = position.Id;
            }

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteEmployee(string employeeCode)
        {
            // 
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);
            if (employee == null) return false;

            // Thay vì _context.Employees.Remove(employee);
            employee.IsActive = false; // Chuyển trạng thái hoạt động thành false
            employee.UpdatedAt = DateTime.Now;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
