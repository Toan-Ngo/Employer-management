using HRMS.Core.Domain.Entities;

namespace HRMS.Core.Interface
{
    public interface ILeaveRequestService
    {
        Task<List<LeaveRequest>> GetLeaveRequests();
        Task<List<LeaveRequest>> GetLeaveRequestByEmployee(int employeeId);
        Task<bool> CreateLeaveRequest(LeaveRequest request);
        Task<bool> UpdateLeaveRequest(LeaveRequest request);
        Task<bool> DeleteLeaveRequest(int id);
    }
}
