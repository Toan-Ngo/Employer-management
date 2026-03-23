using System;

namespace HRMS.Core.DTOs
{
    public class FeedbackDto
    {
        public int Id { get; set; }

        public string EmployeeCode { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string Type { get; set; } = null!;

        public int Status { get; set; } 

        public string? AdminReply { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? RepliedAt { get; set; }
    }
}