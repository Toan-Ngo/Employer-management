using HRMS.Core.Entities;
using HRMS.Core.DTOs;

namespace HRMS.Core.Interfaces.Admin
{
    public interface IPositionService
    {
        Task<List<PositionDto>> GetPositions();
        Task<bool> CreatePosition(PositionDto position);
        Task<bool> UpdatePosition(int id, string newName); 
        Task<bool> DeletePosition(int id); 
    }
}
