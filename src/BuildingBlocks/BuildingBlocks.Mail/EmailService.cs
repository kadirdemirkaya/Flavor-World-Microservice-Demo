using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Models;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net;
using System.Net.Mail;

namespace BuildingBlocks.Mail
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;
        private readonly IConfiguration _configuration;
        private Func<string, string, Task<UserModel?>> _userConfirm;
        private Func<string, Task<UserModel?>> _getUserWithEmail;
        private EmailConnection _emailConnection;

        public EmailService(IConfiguration configuration, Func<string, string, Task<UserModel?>> userConfirm, Func<string, Task<UserModel?>> getUserWithEmail)
        {
            _configuration = configuration;
            _userConfirm = userConfirm;
            _getUserWithEmail = getUserWithEmail;
            _emailConnection = new EmailConnection(_configuration);
            _emailConfig = _emailConnection.GetMailConfiguration();
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using var client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                client.Send(mailMessage);
            }
            catch
            {
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        public async Task<bool> ConfirmEmailAsync(string token, string email)
        {
            var user = await GetUserByEmailAsync(email);
            if (user is not null)
            {
                UserModel? userModel = await _userConfirm(token, email);

                if (userModel is not null)
                    return true;
                return false;
            }
            return false;
        }

        private async Task<UserModel> GetUserByEmailAsync(string email) => await _getUserWithEmail(email);

        public async Task SendMessageAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMessageAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMessageAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
                mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_emailConfig.UserName, "INFO UPDATE", System.Text.Encoding.UTF8);

            System.Net.Mail.SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Host = _emailConfig.SmtpServer;
            await smtp.SendMailAsync(mail);
        }

        public async Task SendMessageWithImageAsync(string[] tos, string subject, string body, bool isBodyHtml, string imagePath)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
                mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_emailConfig.UserName, "INFO UPDATE", System.Text.Encoding.UTF8);
            mail.Attachments.Add(new Attachment(imagePath));

            System.Net.Mail.SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Host = _emailConfig.SmtpServer;
            await smtp.SendMailAsync(mail);
        }
    }
}
