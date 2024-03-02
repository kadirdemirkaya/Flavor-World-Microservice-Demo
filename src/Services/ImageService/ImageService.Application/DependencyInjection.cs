using BuildingBlock.Logger;
using ImageService.Application.Configurations;
using ImageService.Application.Registrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection ImageApplicationServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.LogApplicationRegistrationService(configuration);

            services.MapperRegistrationService();

            services.MediatrRegistration(configuration);

            services.ValidationRegistration(configuration);

            return services;
        }

        public static WebApplicationBuilder ImageApplicationBuilderInjection(this WebApplicationBuilder builder, IConfiguration configuration)
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
