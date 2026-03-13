using HRMS.Core.Domain.Entities;

namespace HRMS.Core.Interface
{
    public interface IPositionService
    {
        Task<List<Position>> GetAllPositions();
        Task<Position> GetPositionById(int id);
        Task<Position> CreatePosition(Position position);
        Task<Position> UpdatePosition(int id, Position position);
        Task<bool> DeletePosition(int id);
    }
}
