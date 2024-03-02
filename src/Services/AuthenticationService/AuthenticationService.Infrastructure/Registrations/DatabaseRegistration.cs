using AuthenticationService.Application.Configurations;
using AuthenticationService.Domain.Aggregate;
using AuthenticationService.Domain.Aggregate.Entities;
using AuthenticationService.Domain.Aggregate.ValueObjects;
using AuthenticationService.Infrastructure.Persistence.Data;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Factory.Factories;
using BuildingBlock.MsSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure.Registrations
{
    public static class Database
    {
        public static IServiceCollection DatabaseServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthenticationDbContext>(options =>
            {
                options.UseSqlServer(GetConfigs.GetDbConnectionString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(AssemblyReference.Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: System.TimeSpan.FromSeconds(30), null);
                });
            });

            var optionsBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>().UseSqlServer(GetConfigs.GetDbConnectionString);
            using var dbContext = new AuthenticationDbContext(optionsBuilder.Options);

            services.MsSQLExtension(GetConfigs.GetMsSqlConfig);

            var sp = GetServiceProvider(services);

            var context = sp.GetRequiredService<AuthenticationDbContext>();

            services.AddScoped<IWriteRepository<User, UserId>>(sp =>
            {
                return DatabaseFactory<User, UserId>.CreateForWriteEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            services.AddScoped<IReadRepository<User, UserId>>(sp =>
            {
                return DatabaseFactory<User, UserId>.CreateForReadEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            services.AddScoped<IWriteRepository<Role, RoleId>>(sp =>
            {
                return DatabaseFactory<Role, RoleId>.CreateForWriteEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            services.AddScoped<IReadRepository<Role, RoleId>>(sp =>
            {
                return DatabaseFactory<Role, RoleId>.CreateForReadEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            services.AddScoped<IWriteRepository<RoleUser, RoleUserId>>(sp =>
            {
                return DatabaseFactory<RoleUser, RoleUserId>.CreateForWriteEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            services.AddScoped<IReadRepository<RoleUser, RoleUserId>>(sp =>
            {
                return DatabaseFactory<RoleUser, RoleUserId>.CreateForReadEntity(GetConfigs.GetDatabaseConfig, context, sp);
            });

            return services;
        }

        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
