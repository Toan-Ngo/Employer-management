using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Core.Domain.Entities
{
    [Table("Departments")]
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string DepartmentName { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
