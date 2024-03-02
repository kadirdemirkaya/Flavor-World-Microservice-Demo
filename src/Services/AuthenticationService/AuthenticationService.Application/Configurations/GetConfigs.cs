using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using Consul;
using Microsoft.Extensions.Configuration;

namespace AuthenticationService.Application.Configurations
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
                string conStr = configuration["DbConnectionString:DbConnection"];
                return conStr;
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
                    ID = AuthenticationService.Domain.Constants.Constant.AuthenticationConsul.ID,
                    Name = AuthenticationService.Domain.Constants.Constant.AuthenticationConsul.Name,
                    Address = AuthenticationService.Domain.Constants.Constant.AuthenticationConsul.Host,
                    Port = int.Parse(AuthenticationService.Domain.Constants.Constant.AuthenticationConsul.Port),
                    Tags = new[] { AuthenticationService.Domain.Constants.Constant.AuthenticationConsul.Name, AuthenticationService.Domain.Constants.Constant.AuthenticationConsul.Tag }
                };
            }
        }

        public static InMemoryConfig GetInMemoryConfig
        {
            get
            {
                IConfiguration configuration = GetConfiguration();
                return new()
                {
                    Connection = configuration["DbConnection"]!,
                    InMemoryType = InMemoryType.Memory,
                    RetryCount = int.Parse(configuration["DbConnectionString:RetryCount"])
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
