using AuthenticationService.Application.Configurations;
using AuthenticationService.Infrastructure.Persistence.Data;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using BuildingBlock.Factory.Factories;
using BuildingBlock.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure.Registrations
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

            var context = sp.GetRequiredService<AuthenticationDbContext>();

            services.AddScoped<ITokenService>(sp =>
            {
                return JwtFactory.CreateTokenService(sp, httpContextAccesor, GetConfigs.GetDatabaseConfig, context);
            });

            return services;
        }

        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
