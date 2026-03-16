using HRMS.Core.Entities;
using HRMS.Core.DTOs;

namespace HRMS.Core.Interfaces.Admin
{
    public interface ISalaryService
    {
        Task<List<SalaryDto>> GetSalaries();
        Task<List<SalaryDto>> GetSalary(string employeeId);
        Task<bool> CreateSalary(CreateSalaryDto salary);
        Task<bool> UpdateSalary(int Id, UpdateSalaryDto salary);
        Task<bool> DeleteSalary(int id);
    }
}