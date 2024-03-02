using BuildingBlock.Base.Configs;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Mail
{
    public class EmailConnection
    {
        private IConfiguration _configuration;

        public EmailConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public EmailConfig GetMailConfiguration() => new()
        {
            From = _configuration["EmailConfiguration:From"],
            Password = _configuration["EmailConfiguration:Password"],
            Port = int.Parse(_configuration["EmailConfiguration:Port"]),
            SmtpServer = _configuration["EmailConfiguration:SmtpServer"],
            UserName = _configuration["EmailConfiguration:Username"]
        };

        public EmailConfig GetMailConfiguration(string from, string password, string port, string smtpServer, string username) => new()
        {
            From = from,
            Password = password,
            Port = int.Parse(port),
            SmtpServer = smtpServer,
            UserName = username
        };
    }
}
