using DebtCollectorQR.Models;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace DebtCollectorQR
{
    public class SmtpEmailFileSender
    {
        private readonly GeneratorSensitiveOptions _genOptions;

        public SmtpEmailFileSender(IOptions<GeneratorSensitiveOptions> options)
        {
            this._genOptions = options.Value;
        }

        public void SendFile(string to, string subject, string bodyHtml, params FileInfo[] attachments)
        {
            var smtpClient = new SmtpClient(_genOptions.SmtpEmailHost)
            {
                Port = _genOptions.SmtpPort,
                Credentials = new NetworkCredential(_genOptions.EmailUsername, _genOptions.EmailPassword),
                EnableSsl = true,
            };

            var mail = new MailMessage(new MailAddress(_genOptions.EmailUsername, _genOptions.EmailFromDisplayName), new MailAddress(to));
            mail.Subject = subject;
            mail.Body = bodyHtml;
            mail.IsBodyHtml = true;

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    mail.Attachments.Add(new Attachment(attachment.FullName));
                }
            }
            smtpClient.Send(mail);
        }
    }
}
