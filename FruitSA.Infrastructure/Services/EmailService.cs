using FruitSA.Domain.Helper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace FruitSA.Infrastructure.Services
{
    public class EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> options) : IEmailSender
    {
        private readonly ILogger<EmailService> _logger = logger;
        private readonly EmailSettings _emailSettings = options.Value;


        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                _logger.LogInformation("Sending email to {email}", email);

                await Execute(email, subject, htmlMessage);

                _logger.LogInformation("Email to {email} sent successfully.", email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email {ex.Message}: {ex.Message}");
            }
        }

        private async Task Execute(string toEmail, string subject, string message)
        {
            var fromEmail = _emailSettings.FromEmail;
            var fromName = _emailSettings.FromName;
            var password = _emailSettings.Password;
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = _emailSettings.Port,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send email: {ex}", ex);
            }
        }
    }
}
