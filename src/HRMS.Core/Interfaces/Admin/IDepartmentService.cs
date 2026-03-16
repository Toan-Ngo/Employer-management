using HRMS.Core.DTOs;
using HRMS.Core.Entities;

namespace HRMS.Core.Interfaces.Admin
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDto>> GetDepartments();
        Task<Department> GetDepartment(string departmentName);
        Task<bool> CreateDepartment(DepartmentDto dto);
        Task<bool> UpdateDepartment(DepartmentDto dto);
        Task<bool> DeleteDepartment(string departmentName);
    }
}