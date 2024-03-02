using AuthenticationService.Application.Abstractions;
using AuthenticationService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure.Registrations
{
    public static class EventService
    {
        public static IServiceCollection EventServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPubEventService, PubEventService>();

            return services;
        }
    }
}
