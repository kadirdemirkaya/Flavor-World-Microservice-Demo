using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BuildingBlock.Logger;
using ImageService.Application.Configurations;

namespace ImageService.Application.Registrations
{
    public static class Logger
    {
        public static IServiceCollection LogApplicationRegistrationService(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }

        public static WebApplicationBuilder LogApplicationRegistrationBuilder(this WebApplicationBuilder builder, IConfiguration configuration)
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
