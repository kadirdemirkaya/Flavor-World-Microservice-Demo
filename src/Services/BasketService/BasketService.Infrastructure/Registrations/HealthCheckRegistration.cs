using BuildingBlock.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Infrastructure.Registrations
{
    public static class HealthCheck
    {
        public static IServiceCollection HealthCheckServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.HealtCheckExtension(configuration);

            return services;
        }
        public static WebApplication HealtCheckAppRegistration(this WebApplication app)
        {
            app.HealtCheckApp();

            return app;
        }
    }
}
