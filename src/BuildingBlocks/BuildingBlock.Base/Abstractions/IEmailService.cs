using BuildingBlock.Base.Models;

namespace BuildingBlock.Base.Abstractions
{
    public interface IEmailService
    {
        void SendEmail(Message message);
        Task<bool> ConfirmEmailAsync(string token, string email);
        Task SendMessageAsync(string to, string subject, string body, bool isBodyHtml = true);
        Task SendMessageAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task SendMessageWithImageAsync(string[] tos, string subject, string body, bool isBodyHtml, string imagePath);
    }
}
