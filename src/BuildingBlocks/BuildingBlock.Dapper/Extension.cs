using BuildingBlock.Logger;
using BuildingBlock.Mapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlock.Dapper
{
    public static class Extension
    {
        public static IServiceCollection DapperExtension(this IServiceCollection services, Assembly assembly)
        {
            services.AddScoped(typeof(IDapperService<,>), typeof(DapperService<,>));

            services.AddScoped(typeof(IDapperService<>), typeof(DapperService<>));

            services.MapperExtension(assembly);

            return services;
        }

        public static WebApplicationBuilder DapperExtensionApp(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.AddLogSeriLogFile(
               new()
               {
                   ApplicationName = configuration["ElasticSearch:ApplicationName"],
                   DefaultIndex = configuration["ElasticSearch:DefaultIndex"],
                   ElasticUrl = configuration["ElasticSearch:ElasticUrl"],
                   Password = configuration["ElasticSearch:Password"],
                   UserName = configuration["ElasticSearch:Username"]
               }, configuration);

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
