using HRMS.Core.Domain.Entities;
using HRMS.Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data.Service
{
    public class SalaryService : ISalaryService
    {
        private readonly HRMSContext _context;
        public SalaryService(HRMSContext context)
        {
            _context = context;
        }
        public async Task<List<Salary>> GetSalaries()
        {
            return await _context.Salaries
                .Include(s => s.Employee)
                .ToListAsync();
        }
        public async Task<bool> CreateSalary(Salary salary)
        {
            await _context.Salaries.AddAsync(salary);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteSalary(int id)
        {
            var salary = await _context.Salaries.FirstOrDefaultAsync(s => s.Id == id);
            if(salary == null)  return false;
            _context.Salaries.Remove(salary); ;
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<List<Salary>> GetSalaryByEmployee(string employeeId)
        {
            var salary = await _context.Salaries.Include(e => e.Employee)
                .Where(s => s.Employee.EmployeeCode.ToLower().Trim() == employeeId.ToLower().Trim()).ToListAsync();
            return salary;


        }

        public async Task<bool> UpdateSalary(int employeeId, Salary salary)
        {
            var existingSalary = await _context.Salaries
        .FirstOrDefaultAsync(s => s.EmployeeId == employeeId);

            if (existingSalary == null)
                return false;

            existingSalary.LuongCoBan = salary.LuongCoBan;
            existingSalary.PhuCap = salary.PhuCap;
            existingSalary.Thuong = salary.Thuong;
            existingSalary.HaoHut = salary.HaoHut;
            existingSalary.NgayTinhLuong = salary.NgayTinhLuong;

            return await _context.SaveChangesAsync() > 0;
        }
}
