using HRMS.Core.Entities;
using HRMS.Core.DTOs;

namespace HRMS.Core.Interfaces.Admin
{
    public interface IPositionService
    {
        Task<List<PositionDto>> GetPositions();
        Task<PositionDto> GetPosition(string positionName);
        Task<bool> CreatePosition(PositionDto position);
        Task<bool> UpdatePosition(PositionDto position);
        Task<bool> DeletePosition(string positionName);
    }
}
