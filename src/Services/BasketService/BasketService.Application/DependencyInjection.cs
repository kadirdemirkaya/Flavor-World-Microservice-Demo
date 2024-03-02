using BasketService.Application.Registrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection BasketApplicationServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.LogRegistrationService(configuration);

            services.MapperRegistrationService();

            services.MediatrRegistration(configuration);

            return services;
        }
    }
}
