using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BuildertrendMVC.Services
{
    public class EmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _from;

        public EmailService(string smtpHost, int smtpPort, string smtpUser, string smtpPass, string from)
        {
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
            _from = from;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MailMessage(_from, to, subject, body);
            message.IsBodyHtml = true;
            using (var client = new SmtpClient(_smtpHost, _smtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                await client.SendMailAsync(message);
            }
        }
    }
}
