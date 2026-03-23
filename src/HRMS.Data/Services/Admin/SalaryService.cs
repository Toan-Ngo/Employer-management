using HRMS.Core.Entities;
using HRMS.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using HRMS.Core.Interfaces.Admin;

namespace HRMS.Data.Services.Admin
{
    public class SalaryService : ISalaryService
    {
        private readonly HRMSContext _context;
        private readonly IUnitOfWork _unitOfWork; 
        public SalaryService(HRMSContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SalaryDto>> GetSalaries()
        {
            return await _context.Payrolls
                .Include(s => s.Employee)
                .Select(s => new SalaryDto
                {
                    Id = s.Id, 
                    EmployeeCode = s.Employee.EmployeeCode,
                    FullName = s.Employee.FirstName + " " + s.Employee.LastName,
                    LuongCoBan = s.LuongCoBan,
                    PhuCap = s.PhuCap,
                    Thuong = s.Thuong,
                    HaoHut = s.HaoHut,
                    LuongThucNhan = s.LuongThucNhan,
                    NgayTinhLuong = s.NgayTinhLuong
                })
                .ToListAsync();
        }

        public async Task<bool> CreateSalary(CreateSalaryDto dto)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeCode.ToLower().Trim() == dto.EmployeeCode.ToLower().Trim());

            if (employee == null)
                return false;

            var salary = new Payroll
            {
                EmployeeId = employee.Id,
                LuongCoBan = dto.LuongCoBan,
                PhuCap = dto.PhuCap,
                Thuong = dto.Thuong,
                HaoHut = dto.HaoHut,
                NgayTinhLuong = DateTime.UtcNow
            };

            salary.LuongThucNhan =
                salary.LuongCoBan
                + (salary.PhuCap ?? 0)
                + (salary.Thuong ?? 0)
                - (salary.HaoHut ?? 0);

            await _context.Payrolls.AddAsync(salary);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteSalary(int id)
        {
            var salary = await _context.Payrolls
                .FirstOrDefaultAsync(s => s.Id == id);

            if (salary == null)
                return false;

            _context.Payrolls.Remove(salary);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<List<SalaryDto>> GetSalary(string employeeCode)
        {
            return await _context.Payrolls
                .Include(s => s.Employee)
                .Where(s => s.Employee.EmployeeCode.ToLower().Trim() == employeeCode.ToLower().Trim())
                .Select(s => new SalaryDto
                {
                    Id = s.Id, 
                    EmployeeCode = s.Employee.EmployeeCode,
                    FullName = s.Employee.FirstName + " " + s.Employee.LastName,
                    LuongCoBan = s.LuongCoBan,
                    PhuCap = s.PhuCap,
                    Thuong = s.Thuong,
                    HaoHut = s.HaoHut,
                    LuongThucNhan = s.LuongThucNhan,
                    NgayTinhLuong = s.NgayTinhLuong
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateSalary(int id, UpdateSalaryDto dto)
        {
            var salary = await _context.Payrolls
                .FirstOrDefaultAsync(s => s.Id == id);

            if (salary == null)
                return false;

            salary.LuongCoBan = dto.LuongCoBan;
            salary.PhuCap = dto.PhuCap;
            salary.Thuong = dto.Thuong;
            salary.HaoHut = dto.HaoHut;

            salary.LuongThucNhan =
                salary.LuongCoBan
                + (salary.PhuCap ?? 0)
                + (salary.Thuong ?? 0)
                - (salary.HaoHut ?? 0);

            salary.NgayTinhLuong = DateTime.UtcNow;

   
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}