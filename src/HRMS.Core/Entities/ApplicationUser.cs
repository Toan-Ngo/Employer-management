using HRMS.Core.Entities;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; }

    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}