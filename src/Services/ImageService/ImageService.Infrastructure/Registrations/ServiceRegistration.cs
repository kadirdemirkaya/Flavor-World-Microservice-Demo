using ImageService.Application.Abstractions;
using ImageService.Infrastructure.Services;
using ImageService.Infrastructure.Services.Background;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageService.Infrastructure.Registrations
{
    public static class Service
    {
        public static IServiceCollection ServiceRegsitration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHostedService<LogCleanupService>();

            services.AddScoped<IImageService, ImageService.Infrastructure.Services.ImageService>();

            services.AddScoped<IImageTypeService, ImageTypeService>();

            return services;
        }
    }
}
