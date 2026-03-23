using HRMS.Core.DTOs;

namespace HRMS.Core.Interfaces.Admin
{
    public interface ILeaveRequestService
    {
        Task<List<LeaveRequestDto>> GetLeaveRequests();
        Task<List<LeaveRequestDto>> GetLeaveRequestByEmployee(string employeeId);
        Task<bool> CreateLeaveRequest(CreateLeaveRequestDto request);
        Task<bool> UpdateLeaveRequest(int id, UpdateLeaveRequestDto request);
        Task<bool> DeleteLeaveRequest(int id);
        // chức năng cho phép hoặc từ chối đơn xin nghỉ
        Task<bool> ApproveLeaveRequest(int id);
        Task<bool> RejectLeaveRequest(int id);
    }
}
