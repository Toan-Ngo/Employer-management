using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Core.Entities
{
    [Table("Salaries")]
    public class Payroll
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; } // kết nối với Employee thông qua khóa ngoại
        public Employee Employee { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal LuongCoBan { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PhuCap { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Thuong { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? HaoHut { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LuongThucNhan { get; set; }

        public DateTime NgayTinhLuong { get; set; } = DateTime.UtcNow;

    }
}
