using HRMS.Core.DTOs;
using HRMS.Core.Entities;
using HRMS.Core.Interfaces.Admin;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data.Services.Admin
{
    public class FeedbackService : IFeedbackService
    {
        private readonly HRMSContext _context;

        public FeedbackService(HRMSContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FeedbackDto>> GetAllFeedbacksAsync()
        {
            return await _context.Feedbacks
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    EmployeeCode = f.EmployeeCode,
                    Title = f.Title,
                    Content = f.Content,
                    Type = f.Type,
                    Status = (int)f.Status,
                    AdminReply = f.AdminReply,
                    CreatedAt = f.CreatedAt,
                    RepliedAt = f.RepliedAt
                })
                .ToListAsync();
        }

        public async Task<bool> CreateFeedbackAsync(string employeeCode, CreateFeedbackDto dto)
        {
            var feedback = new Feedback
            {
                EmployeeCode = employeeCode,
                Title = dto.Title,
                Content = dto.Content,
                Type = dto.Type,
                CreatedAt = DateTime.UtcNow,
                Status = FeedbackStatus.Pending
            };

            _context.Feedbacks.Add(feedback);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ReplyFeedbackAsync(int id, AdminReplyDto dto)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null) return false;

            feedback.AdminReply = dto.ReplyContent;
            feedback.Status = (FeedbackStatus)dto.NewStatus; // Ép từ int của DTO về Enum
            feedback.RepliedAt = DateTime.UtcNow;

            _context.Entry(feedback).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<FeedbackDto>> GetFeedbacksByEmployeeAsync(string employeeCode)
        {
            return await _context.Feedbacks
                .Where(f => f.EmployeeCode == employeeCode)
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    EmployeeCode = f.EmployeeCode,
                    Title = f.Title,
                    Content = f.Content,
                    Type = f.Type,
                    Status = (int)f.Status,
                    AdminReply = f.AdminReply,
                    CreatedAt = f.CreatedAt,
                    RepliedAt = f.RepliedAt
                })
                .ToListAsync();
        }

        public async Task<bool> DeleteFeedbackAsync(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null) return false;

            _context.Feedbacks.Remove(feedback);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}