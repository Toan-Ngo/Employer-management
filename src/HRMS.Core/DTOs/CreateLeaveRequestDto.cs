namespace HRMS.Core.DTOs
{
    public class CreateLeaveRequestDto
    {
        public string EmployeeCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
    }
}
