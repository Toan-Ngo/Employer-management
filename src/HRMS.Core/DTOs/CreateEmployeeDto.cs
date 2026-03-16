namespace HRMS.Core.DTOs
{
    public class CreateEmployeeDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime DateOfBirth { get; set; }

        public GenderType Gender { get; set; } = GenderType.Male;

        public string Address { get; set; }

        public string DepartmentName { get; set; }
        public enum GenderType
        {
            Male,
            Female,
            Other
        }
    }
}
