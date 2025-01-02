using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SprintBoard.Interfaces;

namespace SprintBoard.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost = "smtp.example.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "your-email@example.com";
        private readonly string _smtpPass = "your-password";

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_smtpHost, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(to);
            await client.SendMailAsync(mailMessage);
        }
    }
}
