using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Core.Domain.Entities
{
    [Table("Positions")]
    public class Position
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string PositionName { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    }
}
