using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BuildingBlock.Logger;

namespace BuildingBlock.Elasticsearch
{
    public static class Extension
    {
        public static IServiceCollection ElasticsearchExtension(this IServiceCollection services, SearchConfig config)
        {
            services.AddSingleton<SearchConfig>(config);

            services.AddScoped(typeof(IElastic<>), typeof(ElasticService<>));

            services.AddScoped(typeof(IElastic<,>), typeof(ElasticService<,>));

            services.AddScoped(typeof(ICompleteService<>), typeof(CompleteService<>));

            return services;
        }

        public static WebApplicationBuilder LogRegistrationBuilder(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.AddLogSeriLogConsole(
                new()
                {
                    ApplicationName = configuration["ElasticSearch:ApplicationName"],
                    DefaultIndex = configuration["ElasticSearch:DefaultIndex"],
                    ElasticUrl = configuration["ElasticSearch:ElasticUrl"],
                    Password = configuration["ElasticSearch:Password"],
                    UserName = configuration["ElasticSearch:Username"]
                }, configuration);

            builder.AddConsoleBuilder(config =>
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
