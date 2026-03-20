namespace HRMS.Core.DTOs
{
    public class PositionDto
    {
        public int Id { get; set; } 
        public string PositionName { get; set; }
        public List<EmployeeListDto> Employees { get; set; } = new List<EmployeeListDto>();
    }
    public class EmployeeListDto
    {
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
    }
}