using BasketService.Application.Configurations;
using BuildingBlock.Base.Configs;
using BuildingBlock.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Application.Registrations
{
    public static class Log
    {
        public static IServiceCollection LogRegistrationService(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        public static WebApplicationBuilder LogRegistrationBuilder(this WebApplicationBuilder builder, LogConfig config, IConfiguration configuration)
        {
            builder.AddLogSeriLogFile(GetConfigs.GetLogConfig, configuration);

            builder.AddFileBuilder(config =>
            {
                config.ApplicationName = "OrderService";
                config.DefaultIndex = "Order";
                config.ElasticUrl = configuration["ElasticSearch:ElasticUrl"];
                config.UserName = string.Empty;
                config.Password = string.Empty;
            }, configuration);

            return builder;
        }
    }
}
