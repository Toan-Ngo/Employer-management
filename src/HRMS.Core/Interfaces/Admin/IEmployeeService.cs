using HRMS.Core.DTOs;
using HRMS.Core.Entities;

namespace HRMS.Core.Interfaces.Admin
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetEmployees();
        Task<Employee> GetEmployee(string EmployCode);
        Task<string> GenerateEmployeeCode();
        Task<bool> CreateEmployee(CreateEmployeeDto employee);
        Task<bool> UpdateEmployee( UpdateEmployeeDto dto);
        Task<bool> DeleteEmployee(string id);
    }
}
