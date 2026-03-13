using HRMS.Core.Domain.Entities;

namespace HRMS.Core.Interface
{
    public interface IContractService
    {
        Task<List<Contract>> contracts();
        Task<Contract> GetContractById(int id);
        Task<bool> CreateContract(Contract contract);
        Task<bool> UpdateContract(Contract contract);
        Task<bool> DeleteContract(int id);
    }
}
