using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using Consul;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;

namespace BasketService.Application.Configurations
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
                    ID = BasketService.Domain.Constants.Constant.BasketConsul.ID,
                    Name = BasketService.Domain.Constants.Constant.BasketConsul.Name,
                    Address = BasketService.Domain.Constants.Constant.BasketConsul.Host,
                    Port = int.Parse(BasketService.Domain.Constants.Constant.BasketConsul.Port),
                    Tags = new[] { BasketService.Domain.Constants.Constant.BasketConsul.Name, BasketService.Domain.Constants.Constant.BasketConsul.Tag }
                };
            }
        }

        public static EventBusConfig GetRabbitMqDefaultEventBusConfig
        {
            get
            {
                IConfiguration configuration = GetConfiguration();
                return new()
                {
                    ConnectionRetryCount = int.Parse(configuration["RabbitMq:ConnectionRetryCount"]),
                    EventNameSuffix = configuration["RabbitMq:EventNameSuffix"],
                    SubscriberClientAppName = configuration["RabbitMq:SubscriberClientAppName"],
                    EventBusType = EventBusType.RabbitMQ
                };
            }
        }

        public static EventBusConfig GetRedisDefaultConfig
        {
            get
            {
                IConfiguration configuration = GetConfiguration();
                return new()
                {
                    Connection = $"{configuration["RedisConfig:Host"]}:{configuration["RedisConfig:Port"]}",
                    ConnectionRetryCount = int.Parse(configuration["RedisConfig:ConnectionRetryCount"]),
                    EventNameSuffix = configuration["RedisConfig:EventNameSuffix"],
                    SubscriberClientAppName = configuration["RedisConfig:SubscriberClientAppName"],
                    EventBusType = EventBusType.Redis
                };
            }
        }

        public static EventBusConfig GetRedisEventBusConfig
        {
            get
            {
                IConfiguration configuration = GetConfiguration();
                return new()
                {
                    Connection = GetRedisConfig,
                    ConnectionRetryCount = int.Parse(configuration["RedisConfig:ConnectionRetryCount"]),
                    EventBusConnectionString = configuration["RedisConfig:RedisConnection"],
                    EventBusType = BuildingBlock.Base.Enums.EventBusType.Redis
                };
            }
        }

        public static RedisConfig GetRedisConfig
        {
            get
            {
                IConfiguration configuration = GetConfiguration();
                return new()
                {
                    Connection = configuration["RedisConfig:RedisConnection"],
                    ConnectionRetryCount = int.Parse(configuration["RedisConfig:ConnectionRetryCount"])
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

        private static IConfiguration GetConfiguration() // !!!!
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
