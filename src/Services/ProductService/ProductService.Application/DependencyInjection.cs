using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Registrations;

namespace ProductService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection ProductApplicationServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.LogRegistrationService(configuration);

            services.MapperRegistrationService();

            services.MediatrRegistrationService(configuration);

            services.ValidationRegistrationService(configuration);

            return services;
        }
    }
}
