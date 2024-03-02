using BuildingBlock.Base.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BuildingBlock.Logger;
using ProductService.Domain.Constants;
using ProductService.Application.Configurations;

namespace ProductService.Infrastructure.Registrations
{
    public static class Log
    {
        public static IServiceCollection LogRegistrationService(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        public static WebApplicationBuilder LogRegistrationBuilder(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.AddLogSeriLogFile(
                GetConfigs.GetLogConfig, configuration);

            builder.AddFileBuilder(config =>
            {
                config.ApplicationName = configuration["ElasticSearch:ApplicationName"];
                config.DefaultIndex = configuration["ElasticSearch:ElasticUrl"];
                config.ElasticUrl = configuration["ElasticSearch:DefaultIndex"];
                config.UserName = configuration["ElasticSearch:Username"];
                config.Password = configuration["ElasticSearch:Password"];
            }, configuration);

            return builder;
        }
    }
}
