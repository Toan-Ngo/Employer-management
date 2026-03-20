using HRMS.Core.Entities;
using HRMS.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using HRMS.Core.Interfaces.Admin;

namespace HRMS.Data.Services.Admin
{
    public class PositionService : IPositionService
    {
        private readonly HRMSContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public PositionService(HRMSContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<PositionDto>> GetPositions()
        {
            return await _context.Positions
                .Include(p => p.Employees)
                    .ThenInclude(e => e.Department) 
                .Select(p => new PositionDto
                {
                    Id = p.Id,
                    PositionName = p.PositionName,
                    Employees = p.Employees
                        .Where(e => e.IsActive)
                        .Select(e => new EmployeeListDto
                        {
                            EmployeeCode = e.EmployeeCode,
                            FullName = e.FirstName + " " + e.LastName,
                            DepartmentName = e.Department.DepartmentName,
                            IsActive = e.IsActive
                        }).ToList()
                })
                .ToListAsync();
        }
        public async Task<bool> CreatePosition(PositionDto dto)
        {
            // Chặn trùng tên
            var exists = await _context.Positions.AnyAsync(p => p.PositionName.ToLower() == dto.PositionName.ToLower().Trim());
            if (exists) return false;

            var position = new Position { PositionName = dto.PositionName.Trim() };
            await _context.Positions.AddAsync(position);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePosition(int id, string newName)
        {
            var existing = await _context.Positions.FindAsync(id);
            if (existing == null) return false;

            existing.PositionName = newName.Trim();
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePosition(int id)
        {
            var position = await _context.Positions
                .Include(p => p.Employees)
                .FirstOrDefaultAsync(p => p.Id == id);

            // Không cho xóa nếu chức vụ này đang có nhân viên ngồi
            if (position == null || position.Employees.Any()) return false;

            _context.Positions.Remove(position);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
