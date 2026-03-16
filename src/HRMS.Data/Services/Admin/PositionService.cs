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
                .Select(p => new PositionDto
                {
                    PositionName = p.PositionName
                })
                .ToListAsync();
        }
        public async Task<bool> CreatePosition(PositionDto dto)
        {
            var position = new Position
            {
                PositionName = dto.PositionName
            };

            await _context.Positions.AddAsync(position);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePosition(string positionName)
        {
            var position = await _context.Positions
                           .FirstOrDefaultAsync(p => p.PositionName.ToLower().Trim() == positionName.ToLower().Trim());

            if (position == null)
                return false;

            _context.Positions.Remove(position);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<PositionDto?> GetPosition(string positionName)
        {
            return await _context.Positions
                .Where(p => p.PositionName.ToLower().Trim() == positionName.ToLower().Trim())
                .Select(p => new PositionDto
                {
                    PositionName = p.PositionName
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdatePosition(PositionDto position)
        {
            var existing = await _context.Positions
                .FirstOrDefaultAsync(p => p.PositionName.ToLower().Trim()
                == position.PositionName.ToLower().Trim());
            if (existing == null) return false;
            existing.PositionName = position.PositionName;
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
