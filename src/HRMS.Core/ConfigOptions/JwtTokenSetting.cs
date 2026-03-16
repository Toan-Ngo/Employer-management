namespace HRMS.Core.ConfigOptions
{
    public class JwtTokenSetting
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpireInHours { get; set; }
    }
}
