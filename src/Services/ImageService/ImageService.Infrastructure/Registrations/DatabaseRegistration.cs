using BuildingBlock.Base.Abstractions;
using BuildingBlock.Dapper;
using BuildingBlock.Factory.Factories;
using BuildingBlock.MsSql;
using ImageService.Application.Configurations;
using ImageService.Domain.Aggregate;
using ImageService.Domain.Aggregate.Entities;
using ImageService.Domain.Aggregate.ValueObjects;
using ImageService.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageService.Infrastructure.Registrations
{
    public static class Database
    {
        public static IServiceCollection DatabaseServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ImageDbContext>(options =>
            {
                options.UseSqlServer(GetConfigs.GetDbConnectionString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(AssemblyReference.Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: System.TimeSpan.FromSeconds(30), null);
                });
            });

            var optionsBuilder = new DbContextOptionsBuilder<ImageDbContext>().UseSqlServer(GetConfigs.GetDbConnectionString);
            using var dbContext = new ImageDbContext(optionsBuilder.Options);

            services.MsSQLExtension(GetConfigs.GetMsSqlConfig);

            var sp = GetServiceProvider(services);
            var context = sp.GetRequiredService<ImageDbContext>();

            #region MSSQL
            services.AddScoped<IWriteRepository<Image, ImageId>>(sp =>
            {
                return DatabaseFactory<Image, ImageId>.CreateForWriteEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            services.AddScoped<IReadRepository<Image, ImageId>>(sp =>
            {
                return DatabaseFactory<Image, ImageId>.CreateForReadEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            services.AddScoped<IWriteRepository<ImageUser, ImageUserId>>(sp =>
            {
                return DatabaseFactory<ImageUser, ImageUserId>.CreateForWriteEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            services.AddScoped<IReadRepository<ImageUser, ImageUserId>>(sp =>
            {
                return DatabaseFactory<ImageUser, ImageUserId>.CreateForReadEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });
            #endregion


            #region DAPPER
            services.AddScoped<IWriteRepository<Image, ImageId>>(sp =>
            {
                return DatabaseFactory<Image, ImageId>.CreateForWriteEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });
            services.AddScoped<IReadRepository<Image, ImageId>>(sp =>
            {
                return DatabaseFactory<Image, ImageId>.CreateForReadEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });
            services.AddScoped<IWriteRepository<ImageUser, ImageUserId>>(sp =>
            {
                return DatabaseFactory<ImageUser, ImageUserId>.CreateForWriteEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });
            services.AddScoped<IReadRepository<ImageUser, ImageUserId>>(sp =>
            {
                return DatabaseFactory<ImageUser, ImageUserId>.CreateForReadEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });
            #endregion

            #region SERVICES
            services.DapperExtension(AssemblyReference.Assembly);
            #endregion

            return services;
        }

        public static WebApplicationBuilder MsSqlServiceBuilder(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.MsSqlExtensionBuilder(configuration);

            builder.DapperExtensionApp(configuration);

            return builder;
        }
        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
