namespace HRMS.Core.DTOs
{
    public class EmployeeDto
    {
        public string EmployeeCode { get; set; }

        public string FullName { get; set; }

        public string DepartmentName { get; set; }

        public string PositionName { get; set; }
        public DateTime HireDate { get; set; }
        public bool isActive { get; set; }
    }
}
