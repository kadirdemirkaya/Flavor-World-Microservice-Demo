using AuthenticationService.Application.Abstractions;
using AuthenticationService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure.Registrations
{
    public static class Cookie
    {
        public static IServiceCollection CookieRegistration(this IServiceCollection services)
        {
            services.AddScoped<ICookieService, CookieService>();

            return services;
        }
    }
}
