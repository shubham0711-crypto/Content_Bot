using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ContentBot.Models.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace ContentBot.DAL
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var smtp = new SmtpClient
                {
                    Host = _emailSettings.SmtpHost,
                    EnableSsl = true,
                    Port = _emailSettings.Port,
                    Credentials = GetNetworkCredential()
                };

                var message = GenerateMessage(email, subject, htmlMessage);
                smtp.Send(message);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return Task.CompletedTask;
        }

        private NetworkCredential GetNetworkCredential()
        {
            var networkCredential = new NetworkCredential
            {
                UserName = _emailSettings.SMTPUsername,
                Password = _emailSettings.SMTPPassword
            };
            return networkCredential;
        }

        private MailMessage GenerateMessage(string email, string subject, string body)
        {
            var message = new MailMessage(_emailSettings.SMTPUsername, email)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            };
            return message;
        }
    }
}
