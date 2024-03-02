using AuthenticationService.Application.Abstractions;
using AuthenticationService.Infrastructure.Interceptors;
using AuthenticationService.Infrastructure.Services;
using AuthenticationService.Infrastructure.Services.BackGround;
using AuthenticationService.Infrastructure.Services.Grpc;
using BuildingBlock.Base.Abstractions;
using BuildingBlocks.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure.Registrations
{
    public static class Service
    {
        public static IServiceCollection ServiceRegsitration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ICookieService, CookieService>();

            services.AddSingleton<IHashService, HashService>();

            services.AddScoped<PublishEventInterceptors>();

            services.AddHostedService<LogCleanupService>();

            services.AddScoped<IUserService, UserService>();

            var sp = GetServiceProvider(services);
            var service = sp.GetRequiredService<IUserService>();

            services.AddScoped<IEmailService>(sp =>
            {
                return new EmailService(configuration, service.ConfirmByEmailAsync, service.GetUserByEmailAsync);
            });

            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IUserImageService, UserImageService>();

            services.AddScoped<IImageTypeService, ImageTypeService>();

            return services;
        }

        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
