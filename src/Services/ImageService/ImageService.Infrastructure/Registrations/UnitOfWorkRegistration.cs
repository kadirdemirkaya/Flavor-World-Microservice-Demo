using BuildingBlock.Base.Abstractions;
using BuildingBlock.Factory.Factories;
using ImageService.Application.Configurations;
using ImageService.Domain.Aggregate.Entities;
using ImageService.Domain.Aggregate.ValueObjects;
using ImageService.Infrastructure.Persistence.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageService.Infrastructure.Registrations
{
    public static class UnitOfWord
    {
        public static IServiceCollection IUnitOfWorkRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            var sp = GetServiceProvider(services);

            var context = sp.GetRequiredService<ImageDbContext>();

            services.AddScoped<IUnitOfWork>(sp =>
            {
                return DatabaseFactory.CreateUnitOfWork<ImageUser, ImageUserId>(GetConfigs.GetDatabaseConfig, context);
            });
            return services;
        }

        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
