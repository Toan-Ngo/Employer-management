using HRMS.Core.Domain.Entities;
using HRMS.Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data.Service

{
    public class ContractService : IContractService
    {
        private readonly HRMSContext _context; // sử dụng HRMSContext để tương tác với cơ sở dữ liệu
        public ContractService(HRMSContext context)
        {
            _context = context;
        }
        public async Task<List<Contract>> GetContracts()
        {
            return await _context.Contracts.ToListAsync();
        }

        public async Task<bool> CreateContract(Contract contract) // tạo hợp đồng
        {
            var newContract = new Contract
            {
                EmployeeId = contract.EmployeeId,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                ContractType = contract.ContractType,
                ContractName = contract.ContractName,
                Salary = contract.Salary,
            };
            await _context.Contracts.AddAsync(newContract);

            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> DeleteContract(int id) //  xóa hợp đồng
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
                return false;
            _context.Contracts.Remove(contract);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Contract> GetContractById(int id) // lấy hợp đồng theo id
        {
            return await _context.Contracts.FindAsync(id);
        }

        public async Task<bool> UpdateContract(Contract contract) // cập nhật hợp đồng
        {
            var existingContract = await _context.Contracts.FindAsync(contract.Id);
            if (existingContract == null)
                return false;
            existingContract.EmployeeId = contract.EmployeeId;
            existingContract.StartDate = contract.StartDate;
            existingContract.EndDate = contract.EndDate;
            existingContract.ContractType = contract.ContractType;
            existingContract.ContractName = contract.ContractName;
            existingContract.Salary = contract.Salary;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
