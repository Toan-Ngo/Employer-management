using HRMS.Core.ConfigOptions;
using HRMS.Core.Interfaces.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HRMS.Data.Services.Auth
{
    public class TokenService : ITokenService
    {
        private readonly JwtTokenSetting _jwtTokenSettings;

        public TokenService(IOptions<JwtTokenSetting> jwtTokenSettings)
        {
            _jwtTokenSettings = jwtTokenSettings.Value;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtTokenSettings.Key));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtTokenSettings.Issuer,
                audience: _jwtTokenSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_jwtTokenSettings.ExpireInHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ValidIssuer = _jwtTokenSettings.Issuer,
                ValidAudience = _jwtTokenSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtTokenSettings.Key))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out SecurityToken securityToken);

            var jwtToken = securityToken as JwtSecurityToken;

            if (jwtToken == null ||
                !jwtToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}