using BuildingBlock.Base.Abstractions;
using BuildingBlock.Factory.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Abstractions;
using ProductService.Application.Configurations;
using ProductService.Domain.Models;
using ProductService.Infrastructure.Services;

namespace ProductService.Infrastructure.Registrations
{
    public static class Elastic
    {
        public static IServiceCollection ElasticRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IElastic<ProductElasticModel>>(sp =>
            {
                return ElasticFactory<ProductElasticModel>.CreateForClass(GetConfigs.GetSearchConfig, sp);
            });

            services.AddScoped<ICompleteService<ProductElasticModel>>(sp =>
            {
                return ElasticFactory<ProductElasticModel>.CreateForComplete(GetConfigs.GetSearchConfig, sp);
            });

            services.AddScoped<IFilterService, FilterService>();

            return services;
        }
    }
}
