using HRMS.Core.DTOs;
using HRMS.Core.Entities;
using HRMS.Core.Interfaces.Admin;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data.Services.Admin
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HRMSContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(HRMSContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DepartmentDto>> GetDepartments()
        {
            return await _context.Departments
                .Include(d => d.Employees)
                    .ThenInclude(e => e.Position)
                .Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    DepartmentName = d.DepartmentName,
                    TotalEmployees = d.Employees.Count(e => e.IsActive), // Thêm trường đếm nếu cần
                    Employees = d.Employees
                        .Where(e => e.IsActive)
                        .Select(e => new EmployeeListItemDto
                        {
                            EmployeeCode = e.EmployeeCode,
                            FullName = e.FirstName + " " + e.LastName,
                            PositionName = e.Position != null ? e.Position.PositionName : "Chưa có chức vụ",
                            IsActive = e.IsActive
                        }).ToList()
                })
                .ToListAsync();
        }

        public async Task<bool> CreateDepartment(DepartmentDto dto)
        {
            var exists = await _context.Departments
                .AnyAsync(d => d.DepartmentName.ToLower() == dto.DepartmentName.ToLower().Trim());

            if (exists) return false;

            var newDept = new Department { DepartmentName = dto.DepartmentName.Trim() };
            await _context.Departments.AddAsync(newDept);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateDepartment(int id, string newName)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return false;

            // Kiểm tra xem tên mới có trùng với phòng khác không
            var nameExists = await _context.Departments
                .AnyAsync(d => d.Id != id && d.DepartmentName.ToLower() == newName.ToLower().Trim());
            if (nameExists) return false;

            department.DepartmentName = newName.Trim();
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteDepartment(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            // Chặn xóa nếu phòng đang có nhân viên để tránh lỗi SQL FK
            if (department == null || department.Employees.Any()) return false;

            _context.Departments.Remove(department);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<Department?> GetDepartment(int id)
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id); 
        }
    }
}