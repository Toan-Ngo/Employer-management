namespace HRMS.Core.Interfaces.Auth
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
