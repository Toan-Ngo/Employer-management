using HRMS.Core.DTOs;

namespace HRMS.Core.Interfaces.Admin
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedbackDto>> GetAllFeedbacksAsync();
        Task<IEnumerable<FeedbackDto>> GetFeedbacksByEmployeeAsync(string employeeCode);
        Task<bool> CreateFeedbackAsync(string employeeCode, CreateFeedbackDto dto);
        Task<bool> ReplyFeedbackAsync(int id, AdminReplyDto dto);
        Task<bool> DeleteFeedbackAsync(int id);
    }
}