using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Tekus.Application.Common.Email;
using Tekus.Application.Common.Options;

namespace Tekus.Infrastructure.Email
{
    public class SmtpEmailSender(IOptions<EmailSettings> emailOptions) : IEmailSender
    {
        private readonly EmailSettings _settings = emailOptions.Value;

        public async Task SendAsync(string toAddress, string subject, string body, CancellationToken cancellationToken = default)
        {
            using var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                EnableSsl = _settings.UseSsl
            };

            // Local dev SMTP servers (e.g. Papercut) don't require authentication;
            // only attach credentials when a username is actually configured.
            if (!string.IsNullOrEmpty(_settings.Username))
            {
                client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
            }

            using var message = new MailMessage
            {
                From = new MailAddress(_settings.FromAddress, _settings.FromName),
                Subject = subject,
                Body = body
            };
            message.To.Add(toAddress);

            await client.SendMailAsync(message, cancellationToken);
        }
    }
}
