using BasketService.Application.Abstractions;
using BasketService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Infrastructure.Registrations
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
