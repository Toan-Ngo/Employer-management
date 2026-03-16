using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Core.Entities
{
    [Table("LeaveRequests")]
    public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; } // kết nối với Employee thông qua khóa ngoại
        public Employee Employee { get; set; }
        public DateTime StartDate { get; set; } // Ngày bắt đầu xin nghỉ
        public DateTime EndDate { get; set; }
        [Required]
        [StringLength(200)]
        public string Reason { get; set; } // Lý do xin nghỉ
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending; // Trạng thái (đang chờ, đã duyệt, đã từ chối)
        public enum LeaveStatus
        {
            Pending,
            Approved,
            Rejected
        }
    }
}
