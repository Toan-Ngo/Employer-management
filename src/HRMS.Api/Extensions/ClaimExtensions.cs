using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using HRMS.Core.DTOs;

namespace HRMS.Api.Extensions
{
    public static class ClaimExtensions
    {
        public static void GetPermissions(this List<RoleClaimsDto> allPermissions, Type policy)
        {
            FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo fi in fields)
            {
                var attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                string displayName = fi.GetValue(null).ToString();

                if (attributes.Length > 0)
                {
                    var description = (DescriptionAttribute)attributes[0];
                    displayName = description.Description;
                }

                allPermissions.Add(new RoleClaimsDto
                {
                    Value = fi.GetValue(null).ToString(),
                    Type = "Permission",
                    DisplayName = displayName
                });
            }
        }

        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);

            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }
}