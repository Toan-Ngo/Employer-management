using System.ComponentModel.DataAnnotations;

namespace HRMS.Core.Entities
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EmployeeCode { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Type { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public FeedbackStatus Status { get; set; } = FeedbackStatus.Pending;

        public string? AdminReply { get; set; }

        public DateTime? RepliedAt { get; set; }
    }

    public enum FeedbackStatus
    {
        Pending = 0,
        Processing = 1,
        Resolved = 2
    }
}
