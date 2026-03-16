namespace HRMS.Core.DTOs
{
    public class CreateSalaryDto
    {
        public string EmployeeCode { get; set; }

        public decimal LuongCoBan { get; set; }

        public decimal? PhuCap { get; set; }

        public decimal? Thuong { get; set; }

        public decimal? HaoHut { get; set; }
    }
}