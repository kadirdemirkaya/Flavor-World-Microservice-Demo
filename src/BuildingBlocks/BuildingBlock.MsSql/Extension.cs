using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BuildingBlock.Logger;

namespace BuildingBlock.MsSql
{
    public static class Extension
    {
        public static IServiceCollection MsSQLExtension(this IServiceCollection services, MsSqlConfig msSqlConfig)
        {
            services.AddSingleton<MsSqlConfig>(msSqlConfig);

            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

            services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
            services.AddScoped(typeof(IWriteRepository<,>), typeof(WriteRepository<,>));

            services.AddScoped(typeof(IMsSqlService<,>), typeof(MsSqlService<,>));
            services.AddScoped(typeof(IMsSqlService<>), typeof(MsSqlService<>));

            return services;
        }

        public static WebApplicationBuilder MsSqlExtensionBuilder(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.AddLogSeriLogFile(
                new()
                {
                    ApplicationName = builder.Configuration["ElasticSearch:ApplicationName"],
                    DefaultIndex = builder.Configuration["ElasticSearch:DefaultIndex"],
                    ElasticUrl = builder.Configuration["ElasticSearch:ElasticUrl"],
                    Password = builder.Configuration["ElasticSearch:Password"],
                    UserName = builder.Configuration["ElasticSearch:Username"]
                }, builder.Configuration);

            builder.AddFileBuilder(config =>
            {
                config.ApplicationName = builder.Configuration["ElasticSearch:ApplicationName"];
                config.DefaultIndex = builder.Configuration["ElasticSearch:ElasticUrl"];
                config.ElasticUrl = builder.Configuration["ElasticSearch:DefaultIndex"];
                config.UserName = builder.Configuration["ElasticSearch:Username"];
                config.Password = builder.Configuration["ElasticSearch:Password"];
            }, builder.Configuration);

            return builder;
        }
    }
}
