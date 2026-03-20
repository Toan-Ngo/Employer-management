namespace HRMS.Core.Auth
{
    public class AuthenticatedResult
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
        public List<string> Permissions { get; set; }
    }
}
