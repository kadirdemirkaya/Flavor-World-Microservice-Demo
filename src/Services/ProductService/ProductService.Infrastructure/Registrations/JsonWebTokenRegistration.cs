using BuildingBlock.Base.Abstractions;
using BuildingBlock.Factory.Factories;
using BuildingBlock.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProductService.Infrastructure.Registrations
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

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
