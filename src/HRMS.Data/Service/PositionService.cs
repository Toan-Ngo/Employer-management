using HRMS.Core.Domain.Entities;
using HRMS.Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data.Service
{
    public class PositionService : IPositionService
    {
        private readonly HRMSContext _context;
        public PositionService(HRMSContext context)
        {
            _context = context;
        }
        public async Task<List<Position>> GetAllPositions()
        {
            return await _context.Positions.ToListAsync();
        }
        public async Task<bool> CreatePosition(Position position)
        {
            await _context.Positions.AddAsync(position);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePosition(string positionName)
        {
            var position = await _context.Positions
                           .FirstOrDefaultAsync(p => p.PositionName.ToLower().Trim() == positionName.ToLower().Trim());

            if (position == null)
                return false;

            _context.Positions.Remove(position);

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Position> GetPositionByName(string positionName)
        {
            return await _context.Positions
                .FirstOrDefaultAsync(e => e.PositionName.ToLower().Trim() == positionName.ToLower().Trim());
        }

        public async Task<bool> UpdatePosition(Position position)
        {
            var extension = await _context.Positions.FindAsync(position.Id);
            if(extension == null) return false;
            extension.PositionName = position.PositionName;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
