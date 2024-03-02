using AuthenticationService.Application.Configurations;
using AuthenticationService.Domain.Aggregate;
using AuthenticationService.Domain.Aggregate.ValueObjects;
using AuthenticationService.Infrastructure.Persistence.Data;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Factory.Factories;
using BuildingBlock.InMemory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure.Registrations
{
    public static class InMemory
    {
        public static IServiceCollection InMemoryServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthenticationDbContext>(options => options.UseInMemoryDatabase(GetConfigs.GetDbConnectionString));

            var sp = GetServiceProvider(services);
            var context = sp.GetRequiredService<AuthenticationDbContext>();

            services.InMemoryDbContext<User, UserId>(configuration);

            services.AddScoped<IUnitOfWork>(sp =>
            {
                return DatabaseFactory.CreateUnitOfWork(GetConfigs.GetDatabaseConfig, context);
            });

            services.AddScoped<IInMemoryRepository<User, UserId>>(sp =>
            {
                return InMemoryFactory<User, UserId>.CreateForEntity(GetConfigs.GetInMemoryConfig, context, sp);
            });

            return services;
        }
        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
