using BasketService.Application.Configurations;
using BuildingBlock.Logger;

namespace BasketService.Api.Registrations
{
    public static class Logger
    {
        public static IServiceCollection LogRegistrationService(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }

        public static WebApplicationBuilder LogApiRegistrationBuilder(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.AddLogSeriLogFile(GetConfigs.GetLogConfig, configuration);

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
