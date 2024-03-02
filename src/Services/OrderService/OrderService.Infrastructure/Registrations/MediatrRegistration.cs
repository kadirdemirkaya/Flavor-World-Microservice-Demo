using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OrderService.Infrastructure.Registrations
{
    public static class Mediatr
    {
        public static IServiceCollection MediatrRegistrationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(AssemblyReference.Assembly);

            return services;
        }
    }
}
