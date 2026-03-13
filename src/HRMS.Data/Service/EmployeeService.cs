using HRMS.Core.Domain.Entities;
using HRMS.Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HRMSContext _context; // sử dụng HRMSContext để tương tác với cơ sở dữ liệu
        public EmployeeService(HRMSContext context)
        {
            _context = context;
        }
        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _context.Employees.Include(e => e.Department).ToListAsync();
        }

        public async Task<Employee> GetEmployeeById(string EmployeeId)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeCode.ToLower().Trim() == EmployeeId.ToLower().Trim());
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with id {EmployeeId} not found.");
            }
            return employee;
        }
        public async Task<bool> CreateEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            return await _context.SaveChangesAsync() > 0;
            
        }
        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(employee.Id);
            if(existingEmployee == null)
            {
                throw new KeyNotFoundException($"Employee with id {employee.Id} not found.");
            }
            existingEmployee.EmployeeCode = employee.EmployeeCode;
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Phone = employee.Phone;
            existingEmployee.DateOfBirth = employee.DateOfBirth;
            existingEmployee.Gender = employee.Gender;
            existingEmployee.Address = employee.Address;
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteEmployeeAsync(string EmployeeId)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeCode.ToLower().Trim() == EmployeeId.ToLower().Trim());
            if(employee == null)
            {
                throw new KeyNotFoundException($"Employee with id {EmployeeId} not found.");
            }
            _context.Employees.Remove(employee);
            return await _context.SaveChangesAsync() > 0;

        }  
    }
}
