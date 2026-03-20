using HRMS.Core.DTOs;
using HRMS.Core.Entities;

namespace HRMS.Core.Interfaces.Admin
{
    public interface IDepartmentService
    {
        // Lấy danh sách phòng kèm nhân viên bên trong
        Task<List<DepartmentDto>> GetDepartments();
        Task<Department?> GetDepartment(int id);

        // CRUD
        Task<bool> CreateDepartment(DepartmentDto dto);
        Task<bool> UpdateDepartment(int id, string newName);
        Task<bool> DeleteDepartment(int id);
    }
}