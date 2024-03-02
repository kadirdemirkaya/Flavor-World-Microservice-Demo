using BasketService.Application.Abstractions;
using BasketService.Infrastructure.Services.Grpc;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Infrastructure.Registrations
{
    public static class Service
    {
        public static IServiceCollection ServiceRegsitration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenService, TokenService>();

            //services.AddScoped<IUserInfoService, UserInfoService>();

            services.AddScoped<IUserInfoService>(sp =>
            {
                return new UserInfoService(configuration);
            });

            return services;
        }
    }
}
