public class DepartmentDto
{
    public int Id { get; set; } 
    public string DepartmentName { get; set; }
    public List<EmployeeListItemDto> Employees { get; set; } = new List<EmployeeListItemDto>();
    public int TotalEmployees { get; set; }
}

public class EmployeeListItemDto
{
    public string EmployeeCode { get; set; }
    public string FullName { get; set; }
    public string PositionName { get; set; }
    public bool IsActive { get; set; }
}