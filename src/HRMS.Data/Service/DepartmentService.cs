using HRMS.Core.Domain.Entities;
using HRMS.Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HRMSContext _context; // sử dụng HRMSContext để tương tác với cơ sở dữ liệu
        public DepartmentService(HRMSContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateDepartment(Department department)
        {
            var existingDepartment = await _context.Departments
                    .FirstOrDefaultAsync(d => d.DepartmentName.ToLower().Trim()
                    == department.DepartmentName.ToLower().Trim());
            if (existingDepartment != null)
                return false; // Trả về false nếu đã tồn tại phòng ban với tên này
            await _context.Departments.AddAsync(department);
            return await _context.SaveChangesAsync() > 0; // Trả về true nếu thêm thành công
        }

        public async Task<bool> DeleteDepartment(string departmentName)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentName.ToLower().Trim()
                                                                                    == departmentName.ToLower().Trim());
            if (department == null)
                return false; // Trả về false nếu không tìm thấy phòng ban
            _context.Departments.Remove(department);
            return await _context.SaveChangesAsync() > 0; // Trả về true nếu xóa thành công
        }

        public async Task<List<Department>> GetAllDepartments()
        {
            return await _context.Departments.ToListAsync(); // Trả về danh sách tất cả phòng ban
        }

        public async Task<Department> GetDepartmentById(string departmentName)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentName.ToLower().Trim()
                                                                                    == departmentName.ToLower().Trim());
            if (department == null)
                return null; // Trả về null nếu không tìm thấy phòng ban
            return department; // Trả về phòng ban nếu tìm thấy
        }

        public async Task<bool> UpdateDepartment(Department department)
        {
            var existingDepartment = await _context.Departments.FindAsync(department.Id);
            if (existingDepartment == null)
                return (false); // Trả về false nếu không tìm thấy phòng ban
            existingDepartment.DepartmentName = department.DepartmentName; // Cập nhật tên phòng ban
            return await _context.SaveChangesAsync() > 0; // Trả về true nếu cập nhật thành công
        }
    }
}
