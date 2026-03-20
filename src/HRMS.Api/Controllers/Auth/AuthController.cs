using HRMS.Api.Extensions;
using HRMS.Core.Auth;
using HRMS.Core.DTOs;
using HRMS.Core.Interfaces.Auth;
using HRMS.Core.RBAC;
using HRMS.Data.SeedWorks.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text.Json;

namespace HRMS.Api.Controllers.Auth
{
    [Route("api/admin/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser>  _signInManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser>  signInManager,
            ITokenService tokenService,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }
        [HttpPost("login")]
        public async Task<ActionResult<AuthenticatedResult>> Login([FromBody] LoginRequest request)
        {
            if(request == null) { return BadRequest("Invalid request"); }

            var user = await _userManager.FindByNameAsync(request.UserName);

            if(user == null || !user.IsActive) { return Unauthorized(); }
            if (await _userManager.IsLockedOutAsync(user))
            {
                return Unauthorized("User locked");
            }

            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, true);

            if(!result.Succeeded) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            var permissions = await this.GetPermissionsByUserIdAsync(user.Id.ToString());

            var claims = new List<Claim>
{
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(UserClaims.Id, user.Id),
                new Claim(UserClaims.FirstName, user.Employee?.FirstName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add roles
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Add permissions
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("Permission", permission));
            }

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(30);
            await _userManager.UpdateAsync(user);

            return Ok(new AuthenticatedResult()
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                Permissions = permissions
            });
        }
        private async Task<List<string>> GetPermissionsByUserIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var roles = await _userManager.GetRolesAsync(user);

            var permissions = new List<string>();

            var allPermissions = new List<RoleClaimsDto>();

            if (roles.Contains(Roles.Admin))
            {
                var types = typeof(Permissions).GetTypeInfo().DeclaredNestedTypes;

                foreach (var type in types)
                {
                    allPermissions.GetPermissions(type);
                }

                permissions.AddRange(allPermissions.Select(x => x.Value));
            }
            else
            {
                foreach (var roleName in roles)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);

                    var claims = await _roleManager.GetClaimsAsync(role);

                    var permissionClaims = claims
                        .Where(x => x.Type == "Permission")
                        .Select(x => x.Value);

                    permissions.AddRange(permissionClaims);
                }
            }

            return permissions;
        }
    }
}
