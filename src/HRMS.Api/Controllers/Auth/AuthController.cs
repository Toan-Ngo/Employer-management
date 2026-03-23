using HRMS.Api.Extensions;
using HRMS.Core.Auth;
using HRMS.Core.DTOs;
using HRMS.Core.Interfaces.Admin;
using HRMS.Core.Interfaces.Auth;
using HRMS.Core.RBAC;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;

namespace HRMS.Api.Controllers.Auth
{
    [Route("api/admin/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAccountService _accountService; // Thêm IAccountService

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            RoleManager<IdentityRole> roleManager,
            IAccountService accountService) // Inject vào constructor
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticatedResult>> Login([FromBody] LoginRequest request)
        {
            if (request == null) { return BadRequest("Invalid request"); }

            var user = await _userManager.Users
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.UserName == request.UserName);

            if (user == null || !user.IsActive) { return Unauthorized("Tài khoản không tồn tại hoặc đã bị khóa."); }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return Unauthorized("Tài khoản đang bị khóa tạm thời.");
            }

            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, true);
            if (!result.Succeeded) return Unauthorized("Mật khẩu không chính xác.");
            var roles = await _userManager.GetRolesAsync(user);
            var permissions = await this.GetPermissionsByUserIdAsync(user.Id.ToString());
            var employeeCode = user.Employee?.EmployeeCode ?? "";

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim("Id", user.Id),
                new Claim("EmployeeCode", employeeCode),
                new Claim("FirstName", user.Employee?.FirstName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("Permission", permission));
            }

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(30);
            await _userManager.UpdateAsync(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, 
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddDays(30) 
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);


            return Ok(new AuthenticatedResult()
            {
                Token = accessToken,
                Permissions = permissions,
                RefreshToken = "",
                EmployeeCode = employeeCode
            });
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _accountService.GeneratePasswordResetCodeAsync(request.Email);
            return Ok(new { message = "Nếu email tồn tại trong hệ thống, mã xác nhận đã được gửi." });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _accountService.ResetPasswordAsync(request.Email, request.Code, request.NewPassword);

            if (!result)
            {
                return BadRequest(new { message = "Mã xác nhận không chính xác hoặc đã hết hạn." });
            }

            return Ok(new { message = "Đổi mật khẩu thành công. Vui lòng đăng nhập lại." });
        }

        private async Task<List<string>> GetPermissionsByUserIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<string>();

            var roles = await _userManager.GetRolesAsync(user);
            var permissions = new List<string>();
            if (roles.Contains(Roles.Admin))
            {
                var allPermissionsDto = new List<RoleClaimsDto>();
                var types = typeof(Permissions).GetTypeInfo().DeclaredNestedTypes;

                foreach (var type in types)
                {
                    allPermissionsDto.GetPermissions(type);
                }
                permissions.AddRange(allPermissionsDto.Select(x => x.Value));
            }
            else
            {
                foreach (var role in roles)
                {
                    if (RolePermissions.RolesMap.TryGetValue(role, out var rolePermissions))
                    {
                        permissions.AddRange(rolePermissions);
                    }
                }
            }

            return permissions.Distinct().ToList();
        }
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticatedResult>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Không tìm thấy Refresh Token.");
            }

            var user = await _userManager.Users
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return Unauthorized("Token không hợp lệ hoặc đã hết hạn.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var permissions = await this.GetPermissionsByUserIdAsync(user.Id.ToString());
            var employeeCode = user.Employee?.EmployeeCode ?? "";

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim("Id", user.Id),
                new Claim("EmployeeCode", employeeCode),
                new Claim("FirstName", user.Employee?.FirstName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var role in roles) { claims.Add(new Claim(ClaimTypes.Role, role)); }
            foreach (var permission in permissions) { claims.Add(new Claim("Permission", permission)); }

            var newAccessToken = _tokenService.GenerateAccessToken(claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(30);
            await _userManager.UpdateAsync(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddDays(30)
            };
            Response.Cookies.Delete("refreshToken");
            Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);

            return Ok(new AuthenticatedResult()
            {
                Token = newAccessToken,
                RefreshToken = "", 
                Permissions = permissions,
                EmployeeCode = employeeCode
            });
        }
    }
}