using AuthenticationService.Application.Abstractions;
using AuthenticationService.Application.Configurations;
using AuthenticationService.Domain.Aggregate;
using AuthenticationService.Domain.Aggregate.Entities;
using AuthenticationService.Domain.Aggregate.ValueObjects;
using AuthenticationService.Domain.Constants;
using AuthenticationService.Infrastructure.Persistence.Data;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using BuildingBlock.Factory.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure.Registrations
{
    public static class UnitOfWork
    {
        public static IServiceCollection IUnitOfWorkRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            var sp = GetServiceProvider(services);

            var context = sp.GetRequiredService<AuthenticationDbContext>();

            var pubService = sp.GetRequiredService<IPubEventService>();

            Func<string, Task> pubEvent = pubService.PublishDomainEventAsync;

            services.AddScoped<IUnitOfWork>(sp =>
            {
                return DatabaseFactory.CreateUnitOfWork<User, UserId>(GetConfigs.GetDatabaseConfig, context, pubService.PublishDomainEventAsync, Constant.App.ApplicationName);
            });

            services.AddScoped<IUnitOfWork>(sp =>
            {
                return DatabaseFactory.CreateUnitOfWork<Role, RoleId>(GetConfigs.GetDatabaseConfig, context, pubService.PublishDomainEventAsync, Constant.App.ApplicationName);
            });

            services.AddScoped<IUnitOfWork>(sp =>
            {
                return DatabaseFactory.CreateUnitOfWork<RoleUser, RoleUserId>(GetConfigs.GetDatabaseConfig, context, pubService.PublishDomainEventAsync, Constant.App.ApplicationName);
            });

            return services;
        }
        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
