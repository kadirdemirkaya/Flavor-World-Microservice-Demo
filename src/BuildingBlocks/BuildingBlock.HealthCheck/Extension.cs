using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlock.HealthCheck
{
    public static class Extension
    {
        public static IServiceCollection HealtCheckExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks();

            services.AddHealthChecks()
                .AddSqlServer(configuration["DbConnectionString:DbConnection"], name: "MSSQL-Health-Check");

            services.AddHealthChecks()
                .AddRedis($"{configuration["RedisConfig:Host"]}:{configuration["RedisConfig:Port"]}", name: "Redis-Health-Check");

            services.AddHealthChecks()
                .AddRabbitMQ(configuration["RabbitMq:RabbitUrl"], name: "RabbitMq-Healt-Check");

            services.AddHealthChecks()
                .AddElasticsearch(configuration["ElasticSearch:ElasticUrl"], name: "Elasticsearch-Healt-Check");

            return services;
        }

        public static WebApplication HealtCheckApp(this WebApplication app)
        {
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            });

            return app;
        }
    }
}
