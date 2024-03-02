using BuildingBlock.Base.Configs;
using BuildingBlock.Logger;

namespace AuthenticationService.Api.Registration
{
    public static class Logger
    {
        public static IServiceCollection LogRegistrationService(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }

        public static WebApplicationBuilder LogApiRegistrationBuilder(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.AddLogSeriLogFile(
                GetLogConfig(configuration), configuration);

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

        private static LogConfig GetLogConfig(IConfiguration configuration)
             => new()
             {
                 ApplicationName = configuration["ElasticSearch:ApplicationName"],
                 DefaultIndex = configuration["ElasticSearch:DefaultIndex"],
                 ElasticUrl = configuration["ElasticSearch:ElasticUrl"],
                 Password = configuration["ElasticSearch:Password"],
                 UserName = configuration["ElasticSearch:Username"]
             };
    }
}
