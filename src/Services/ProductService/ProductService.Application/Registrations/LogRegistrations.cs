using BuildingBlock.Base.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BuildingBlock.Logger;
using ProductService.Domain.Constants;
using ProductService.Application.Configurations;

namespace ProductService.Application.Registrations
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
                config.ApplicationName = Constant.ProductService;
                config.DefaultIndex = Constant.TableNames.Products;
                config.ElasticUrl = configuration["ElasticSearch:ElasticUrl"];
                config.UserName = string.Empty;
                config.Password = string.Empty;
            }, configuration);

            return builder;
        }
    }
}
