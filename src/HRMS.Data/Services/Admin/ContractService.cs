using HRMS.Core.DTOs;
using HRMS.Core.Entities;
using HRMS.Core.Interfaces.Admin;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data.Services.Admin

{
    public class ContractService : IContractService
    {
        private readonly HRMSContext _context; // sử dụng HRMSContext để tương tác với cơ sở dữ liệu
        private readonly IUnitOfWork _unitOfWork; // sử dụng IUnitOfWork để quản lý giao dịch
        public ContractService(HRMSContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ContractDto>> GetContracts()
        {
            return await _context.Contracts
                .Include(c => c.Employee)
                .Select(c => new ContractDto
                {
                    Id = c.Id,
                    EmployeeCode = c.Employee.EmployeeCode,
                    FullName = c.Employee.LastName + " " + c.Employee.FirstName,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    ContractType = c.ContractType,
                    ContractName = c.ContractName,
                    Salary = c.Salary
                })
                .ToListAsync();
        }

        public async Task<ContractDto?> GetContractById(int id)
        {
            var c = await _context.Contracts
                .Include(c => c.Employee)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (c == null) return null;

            return new ContractDto
            {
                Id = c.Id,
                EmployeeCode = c.Employee.EmployeeCode,
                FullName = c.Employee.LastName + " " + c.Employee.FirstName,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                ContractType = c.ContractType,
                ContractName = c.ContractName,
                Salary = c.Salary
            };
        }

        public async Task<bool> CreateContract(CreateContractDto dto)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeCode == dto.EmployeeCode);

            if (employee == null) return false;

            var newContract = new Contract
            {
                EmployeeId = employee.Id,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ContractType = dto.ContractType,
                ContractName = dto.ContractName,
                Salary = dto.Salary,
                Status = dto.Status ?? "Hiệu lực" 
            };

            await _context.Contracts.AddAsync(newContract);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteContract(int id) //  xóa hợp đồng
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
                return false;
            _context.Contracts.Remove(contract);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateContract(int contractId, UpdateContractDto dto) // cập nhật hợp đồng
        {
            var contract = await _context.Contracts.FindAsync(contractId);

            if (contract == null)
                return false;

            contract.EndDate = dto.EndDate;
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
