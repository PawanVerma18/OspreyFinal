using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Osprey3.Services
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly bool _enableSsl;

        public EmailService(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword, bool enableSsl)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
            _enableSsl = enableSsl;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                    client.EnableSsl = _enableSsl;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpUsername),
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = false
                    };
                    mailMessage.To.Add(email);

                    await client.SendMailAsync(mailMessage);

                    // Log success
                    Console.WriteLine($"Email sent to {email} successfully.");
                }
            }
            catch (Exception ex)
            {
                // Log the full error details
                Console.WriteLine($"Error sending email to {email}: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw; // Re-throw the exception to propagate it
            }
        }
    }
}