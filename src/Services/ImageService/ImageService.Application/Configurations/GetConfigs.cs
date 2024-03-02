using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using Consul;
using Microsoft.Extensions.Configuration;

namespace ImageService.Application.Configurations
{
    public static class GetConfigs
    {
        public static DatabaseConfig GetDatabaseConfig
        {
            get
            {
                IConfiguration configuration = GetConfiguration();
                return new()
                {
                    ConnectionString = configuration["DbConnection"],
                    DatabaseType = DatabaseType.MsSQL,
                    RetryCount = int.Parse(configuration["DbConnectionString:RetryCount"])
                };
            }
        }

        public static MsSqlConfig GetMsSqlConfig
        {
            get
            {
                IConfiguration configuration = GetConfiguration();
                return new()
                {
                    ConnectionString = configuration["DbConnection"],
                    ConnectionRetryCount = int.Parse(configuration["DbConnectionString:RetryCount"]),
                    TrustedConnection = bool.Parse(configuration["DbConnectionString:TrustedConnection"])
                };
            }
        }

        public static string GetDbConnectionString
        {
            get
            {
                IConfiguration configuration = GetConfiguration();
                return configuration["DbConnection"];
            }
        }

        public static string GetConsulConnectionString
        {
            get
            {
                IConfiguration configuration = GetConfiguration();
                return configuration["ConsulConfig:Address"];
            }
        }

        public static LogConfig GetLogConfig
        {
            get
            {
                IConfiguration configuration = GetConfiguration();
                return new()
                {
                    ApplicationName = configuration["ElasticSearch:ApplicationName"],
                    DefaultIndex = configuration["ElasticSearch:DefaultIndex"],
                    ElasticUrl = configuration["ElasticSearch:ElasticUrl"],
                    Password = configuration["ElasticSearch:Password"],
                    UserName = configuration["ElasticSearch:Username"]
                };
            }
        }

        public static AgentServiceRegistration GetAgentServiceRegistration
        {
            get
            {
                return new()
                {
                    ID = ImageService.Domain.Constants.Constant.ImageConsul.ID,
                    Name = ImageService.Domain.Constants.Constant.ImageConsul.Name,
                    Address = ImageService.Domain.Constants.Constant.ImageConsul.Host,
                    Port = int.Parse(ImageService.Domain.Constants.Constant.ImageConsul.Port),
                    Tags = new[] { ImageService.Domain.Constants.Constant.ImageConsul.Name, ImageService.Domain.Constants.Constant.ImageConsul.Tag }
                };
            }
        }

        private static IConfiguration GetConfiguration()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            return new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .Build();
        }
    }
}
