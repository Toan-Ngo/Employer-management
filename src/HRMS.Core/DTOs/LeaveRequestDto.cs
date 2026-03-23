using System;

namespace HRMS.Core.DTOs
{
    public class LeaveRequestDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }

        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}