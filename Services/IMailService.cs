namespace API_Demo.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string mailTo, string subject, string body, List<IFormFile>? attachments = null);
        Task SendEmailUsingDotNet(string mailTo, string subject, string body, List<IFormFile>? attachments = null);
    }
}
