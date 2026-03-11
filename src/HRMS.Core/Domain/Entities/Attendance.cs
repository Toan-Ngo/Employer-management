using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Core.Domain.Entities
{
    [Table("Attendances")]
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        public DateTime AttendanceDate { get; set; }
        [Required]
        [StringLength(50)]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; } // kết nối với Employee thông qua khóa ngoại
        public Employee Employee { get; set; }
    }
}
