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
                    PositionName = e.Position.PositionName
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
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentName.ToLower().Trim()
                == dto.DepartmentName.ToLower().Trim());

            if (department == null)
                return false;

            var employee = new Employee
            {
                EmployeeCode = await GenerateEmployeeCode(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                DateOfBirth = dto.DateOfBirth,
                Gender = (Employee.GenderType)dto.Gender,
                Address = dto.Address,
                DepartmentId = department.Id
            };
            await _context.Employees.AddAsync(employee);
            return await _unitOfWork.SaveChangesAsync() > 0;
            
        }
        public async Task<bool> UpdateEmployee(string employeeCode, UpdateEmployeeDto dto)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeCode.ToLower().Trim()
                == employeeCode.ToLower().Trim());

            if (employee == null)
                return false;

            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentName.ToLower().Trim()
                == dto.DepartmentName.ToLower().Trim());

            if (department == null)
                return false;

            var position = await _context.Positions
                .FirstOrDefaultAsync(p => p.PositionName.ToLower().Trim()
                == dto.PositionName.ToLower().Trim());

            if (position == null)
                return false;

            employee.DepartmentId = department.Id;
            employee.PositionId = position.Id;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteEmployee(string EmployeeId)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeCode.ToLower().Trim() == EmployeeId.ToLower().Trim());
            if(employee == null)
            {
                return false;
            }
            _context.Employees.Remove(employee);
            return await _unitOfWork.SaveChangesAsync() > 0;

        }  
    }
}
