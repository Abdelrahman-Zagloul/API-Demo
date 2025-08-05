using API_Demo.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace API_Demo.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        //using MailKit and Mine kit
        public async Task SendEmailAsync(string mailTo, string subject, string body, List<IFormFile>? attachments = null)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = subject,
            };
            email.To.Add(MailboxAddress.Parse(mailTo));
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

            // for convert the attachments
            var builder = new BodyBuilder();
            if (attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                        builder.Attachments.Add(file.FileName, fileBytes, MimeKit.ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        //using .net
        public async Task SendEmailUsingDotNet(string mailTo, string subject, string body, List<IFormFile>? attachments = null)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_mailSettings.Email, _mailSettings.DisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            message.To.Add(mailTo);

            var attachmentStreams = new List<MemoryStream>();
            if (attachments != null && attachments.Any())
            {
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                        message.Attachments.Add(new Attachment(memoryStream, file.FileName, file.ContentType));
                        attachmentStreams.Add(memoryStream);
                    }
                }
            }

            using var smtpClient = new System.Net.Mail.SmtpClient()
            {
                Host = _mailSettings.Host,
                Port = _mailSettings.Port,
                EnableSsl = true,
                Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password)
            };
            await smtpClient.SendMailAsync(message);

            foreach (var stream in attachmentStreams)
                stream.Dispose();
        }
    }
}
