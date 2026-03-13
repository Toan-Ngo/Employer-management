using HRMS.Core.Domain.Entities;

namespace HRMS.Core.Interface
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllDepartments();
        Task<Department> GetDepartmentById(int id);
        Task<bool> CreateDepartment(Department department);
        Task<bool> UpdateDepartment(Department department);
        Task<bool> DeleteDepartment(int id);
    }
}