using System;

namespace HRMS.Core.DTOs
{
    public class LeaveRequestDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        // Thêm các trường này để tiện hiển thị trên UI bên Angular
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }

        public string Status { get; set; }
    }
}