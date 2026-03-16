using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Core.Entities
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string EmployeeCode { get; set; }
        [Required]
        [MaxLength(50)]
        public string  FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        [Phone]
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderType Gender { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [Required]
        [ForeignKey("Department")]
        public int DepartmentId { get; set; } // kết nối với Department(phòng ban) thông qua khóa ngoại
        public Department Department { get; set; }
        [Required]
        [ForeignKey("Position")]
        public int PositionId { get; set; } // kết nối với Position(chức vụ) thông qua khóa ngoại
        public Position Position { get; set; }
        [Required]
        public DateTime HireDate { get; set; } // ngày tuyển dụng

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>(); // 
        public ICollection<Payroll> Salaries { get; set; } = new List<Payroll>();
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();

        public enum GenderType
        {
            Male,
            Female,
            Other
        }
    }
}
