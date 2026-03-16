using HRMS.Core.DTOs;
using HRMS.Core.Entities;
using HRMS.Core.Interfaces.Admin;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data.Services.Admin
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HRMSContext _context; // sử dụng HRMSContext để tương tác với cơ sở dữ liệu
        private readonly IUnitOfWork _unitOfWork; // sử dụng IUnitOfWork để quản lý giao dịch
        public DepartmentService(HRMSContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateDepartment(DepartmentDto dto)
        {
            var existingDepartment = await _context.Departments
                    .FirstOrDefaultAsync(d => d.DepartmentName.ToLower().Trim()
                    == dto.DepartmentName.ToLower().Trim());
            if (existingDepartment != null)
                return false; // Trả về false nếu đã tồn tại phòng ban với tên này
            var newDepartment = new Department
            {
                DepartmentName = dto.DepartmentName.Trim()
            };
            await _context.Departments.AddAsync(newDepartment);
            return await _unitOfWork.SaveChangesAsync() > 0; // Trả về true nếu thêm thành công
        }

        public async Task<bool> DeleteDepartment(string departmentName)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentName.ToLower().Trim()
                                                                                    == departmentName.ToLower().Trim());
            if (department == null)
                return false; // Trả về false nếu không tìm thấy phòng ban
            _context.Departments.Remove(department);
            return await _unitOfWork.SaveChangesAsync() > 0; // Trả về true nếu xóa thành công
        }

        public async Task<List<DepartmentDto>> GetDepartments()
        {
            return await _context.Employees
             .Include(e => e.Department)
             .Select(e => new DepartmentDto
             {
                 EmployeeCode = e.EmployeeCode,
                 FullName = e.FirstName + " " + e.LastName,
                 DepartmentName = e.Department.DepartmentName
             })
             .ToListAsync();
        }

        public async Task<Department?> GetDepartment(string departmentName)
        {
            var name = departmentName.Trim().ToLower();

            return await _context.Departments
                .Include(d => d.Employees) // lấy tất cả nhân viên trong phòng ban
                .FirstOrDefaultAsync(d => d.DepartmentName.ToLower() == name);
        }

        public async Task<bool> UpdateDepartment(DepartmentDto dto)
        {
            var name = dto.DepartmentName.Trim().ToLower();
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentName.ToLower().Trim() == name);

            if (department == null)
                return (false); // Trả về false nếu không tìm thấy phòng ban
            department.DepartmentName = department.DepartmentName; // Cập nhật tên phòng ban
            return await _unitOfWork.SaveChangesAsync() > 0; // Trả về true nếu cập nhật thành công
        }
    }
}
