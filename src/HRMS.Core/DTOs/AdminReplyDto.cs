using HRMS.Core.Entities; 

namespace HRMS.Core.DTOs
{
    public class AdminReplyDto
    {
        public string ReplyContent { get; set; }
        public FeedbackStatus NewStatus { get; set; } = FeedbackStatus.Resolved;
    }
}
