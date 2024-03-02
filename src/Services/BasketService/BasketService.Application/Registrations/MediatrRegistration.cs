using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Application.Registrations
{
    public static class Mediatr
    {
        public static IServiceCollection MediatrRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(AssemblyReference.Assembly);

            return services;
        }
    }
}
