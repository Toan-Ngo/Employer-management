namespace HRMS.Core.DTOs
{
    public class ContractDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ContractType { get; set; }
        public string ContractName { get; set; }
        public decimal Salary { get; set; }
        public string Status { get; set; }
    }
}
