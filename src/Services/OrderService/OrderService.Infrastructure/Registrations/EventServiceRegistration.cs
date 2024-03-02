using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Abstractions;
using OrderService.Infrastructure.Services;

namespace OrderService.Infrastructure.Registrations
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
