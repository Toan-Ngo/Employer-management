using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Core.Entities
{
    [Table("Contracts")]
    public class Contract
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; } // kết nối với Employee thông qua khóa ngoại
        public Employee Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public string ContractType { get; set; } // Loại hợp đồng (vô thời hạn, có thời hạn, thử việc, v.v.)
        [Required]
        public string ContractName { get; set; } // Tên hợp đồng ( Hợp đồng lao động, Hợp đồng thử việc, v.v.)
        public Decimal Salary { get; set; } // Mức lương theo hợp đồng
        [Required]
        public string Status { get; set; } = "Hiệu lực";
    }
}
