using HRMS.Core.Entities;

namespace HRMS.Core.Interfaces.Admin
{
    public interface ILeaveRequestService
    {
        Task<List<LeaveRequest>> GetLeaveRequests();
        Task<List<LeaveRequest>> GetLeaveRequestByEmployee(string employeeId);
        Task<bool> CreateLeaveRequest(LeaveRequest request);
        Task<bool> UpdateLeaveRequest(LeaveRequest request);
        Task<bool> DeleteLeaveRequest(int id);
        // chức năng cho phép hoặc từ chối đơn xin nghỉ
        Task<bool> ApproveLeaveRequest(int id);
        Task<bool> RejectLeaveRequest(int id);
    }
}
