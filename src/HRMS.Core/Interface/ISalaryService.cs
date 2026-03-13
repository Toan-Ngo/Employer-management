using HRMS.Core.Domain.Entities;

namespace HRMS.Core.Interface
{
    public interface ISalaryService
    {
        Task<List<Salary>> GetSalaries();
        Task<List<Salary>> GetSalaryByEmployee(int employeeId);
        Task<bool> CreateSalary(Salary salary);
        Task<bool> UpdateSalary(Salary salary);
        Task<bool> DeleteSalary(int id);
    }
}