namespace HRMS.Core.DTOs
{
    public class CreateContractDto
    {
        public string EmployeeCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ContractType { get; set; }
        public string ContractName { get; set; }
        public decimal Salary { get; set; }

    }
}
