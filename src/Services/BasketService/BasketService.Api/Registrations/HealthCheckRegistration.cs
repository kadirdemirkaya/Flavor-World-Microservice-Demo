using BuildingBlock.HealthCheck;

namespace BasketService.Api.Registrations
{
    public static class HealthCheck
    {
        public static IServiceCollection HealthCheckServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.HealtCheckExtension(configuration);

            return services;
        }

        public static WebApplication HealthCheckWebApplicationRegistration(this WebApplication app)
        {
            app.HealtCheckApp();

            return app;
        }
    }
}
