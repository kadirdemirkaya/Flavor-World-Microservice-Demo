using BasketService.Application.Configurations;
using BasketService.Infrastructure.Persistence.Data;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Factory.Factories;
using BuildingBlock.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Infrastructure.Registrations
{
    public static class JsonWebToken
    {
        public static IServiceCollection JsonWebTokenServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.JwtExtension(configuration);

            services.AddScoped<IJwtTokenGenerator>(sp =>
            {
                return JwtFactory.CreateJwtTokenGenerator(configuration);
            });

            var sp = GetServiceProvider(services);
            var httpContextAccesor = sp.GetRequiredService<IHttpContextAccessor>();
            var context = sp.GetRequiredService<BasketDbContext>();

            services.AddScoped<ITokenService>(sp =>
            {
                return JwtFactory.CreateTokenService(sp, httpContextAccesor, GetConfigs.GetDatabaseConfig, context);
            });

            return services;
        }
        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
