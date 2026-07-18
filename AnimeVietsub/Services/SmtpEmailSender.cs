using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace AnimeVietsub.Services
{
    // Cau hinh SMTP lay tu appsettings.json (muc EmailSettings)
    public class EmailSettings
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public string TenNguoiGui { get; set; }
    }

    public class SmtpEmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;

        public SmtpEmailSender(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task GuiEmailAsync(string diaChiNhan, string tieuDe, string noiDungHtml)
        {
            using var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
            {
                Credentials = new NetworkCredential(_settings.SmtpUser, _settings.SmtpPass),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(_settings.SmtpUser, _settings.TenNguoiGui),
                Subject = tieuDe,
                Body = noiDungHtml,
                IsBodyHtml = true
            };
            message.To.Add(diaChiNhan);

            await client.SendMailAsync(message);
        }
    }
}
