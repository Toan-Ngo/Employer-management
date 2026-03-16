namespace HRMS.Core.DTOs
{
    public class UpdateAttendanceDto
    {
        public int Id { get; set; }

        public DateTime? CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }
    }
}
