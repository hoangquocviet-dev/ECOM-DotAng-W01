using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace phase_1.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendVerificationEmail(string toEmail, string otpCode)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var smtpServer = emailSettings["SmtpServer"];
            var smtpPort = int.Parse(emailSettings["SmtpPort"] ?? "587");
            var senderEmail = emailSettings["SenderEmail"];
            var senderPassword = emailSettings["SenderPassword"];
            var senderName = emailSettings["SenderName"];

            var smtpClient = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail!, senderName),
                Subject = "Xác thực tài khoản của bạn",
                Body = $"Chào bạn, mã OTP xác thực tài khoản của bạn là: <strong>{otpCode}</strong>",
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);
            smtpClient.Send(mailMessage);
        }

        public void SendPasswordResetEmail(string toEmail, string otpCode)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var smtpServer = emailSettings["SmtpServer"];
            var smtpPort = int.Parse(emailSettings["SmtpPort"] ?? "587");
            var senderEmail = emailSettings["SenderEmail"];
            var senderPassword = emailSettings["SenderPassword"];
            var senderName = emailSettings["SenderName"];

            var smtpClient = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail!, senderName),
                Subject = "Khôi phục mật khẩu của bạn",
                Body = $"Chào bạn, mã OTP để đặt lại mật khẩu của bạn là: <strong>{otpCode}</strong>",
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);
            smtpClient.Send(mailMessage);
        }
    }
}