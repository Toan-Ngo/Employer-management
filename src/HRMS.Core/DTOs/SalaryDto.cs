namespace HRMS.Core.DTOs
{
    public class SalaryDto
    {
        public string EmployeeCode { get; set; }

        public string FullName { get; set; }

        public decimal LuongCoBan { get; set; }

        public decimal? PhuCap { get; set; }

        public decimal? Thuong { get; set; }

        public decimal? HaoHut { get; set; }

        public decimal LuongThucNhan { get; set; }

        public DateTime NgayTinhLuong { get; set; }
    }
}