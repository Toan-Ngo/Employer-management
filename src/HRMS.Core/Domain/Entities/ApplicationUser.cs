using HRMS.Core.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; }

    public DateTime CreatedAt { get; set; }
}