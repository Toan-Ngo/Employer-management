using System.Text.Json.Serialization;

namespace HRMS.Core.DTOs
{
    public class UpdateEmployeeDto
    {
        [JsonPropertyName("employeeCode")] // Chấp nhận chữ 'e' thường từ Angular
        public string EmployeeCode { get; set; }

        [JsonPropertyName("departmentName")]
        public string DepartmentName { get; set; }

        [JsonPropertyName("positionName")]
        public string PositionName { get; set; }
    }
}