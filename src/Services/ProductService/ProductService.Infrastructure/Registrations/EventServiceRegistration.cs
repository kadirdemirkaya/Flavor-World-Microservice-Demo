using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Abstractions;
using ProductService.Infrastructure.Services;

namespace ProductService.Infrastructure.Registrations
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
