using HRMS.Core.DTOs;
using HRMS.Core.Entities;

namespace HRMS.Core.Interfaces.Admin
{
    public interface IContractService
    {
        Task<List<ContractDto>> GetContracts();
        Task<ContractDto> GetContractById(int id);
        Task<bool> CreateContract(CreateContractDto dto);
        Task<bool> UpdateContract(int contractId, UpdateContractDto dto);
        Task<bool> DeleteContract(int id);
    }
}
