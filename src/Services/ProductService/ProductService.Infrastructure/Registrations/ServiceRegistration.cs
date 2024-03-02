using BuildingBlock.Base.Abstractions;
using BuildingBlock.Jwt;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Abstractions;
using ProductService.Infrastructure.Services;
using ProductService.Infrastructure.Services.Background;
using ProductService.Infrastructure.Services.Grpc;

namespace ProductService.Infrastructure.Registrations
{
    public static class Service
    {
        public static IServiceCollection ServiceRegistration(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddHostedService<LogCleanupService>();

            services.AddScoped(typeof(ISessionService<>), typeof(SessionService<>));

            services.AddScoped<IProductImageService, ProductImageService>();

            services.AddScoped<IImageTypeService, ImageTypeService>();

            return services;
        }
    }
}
