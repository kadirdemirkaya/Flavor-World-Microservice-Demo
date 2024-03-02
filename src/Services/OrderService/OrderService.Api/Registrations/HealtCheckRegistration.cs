using BuildingBlock.HealthCheck;

namespace OrderService.Api.Registrations
{
    public static class HealthCheck
    {
        public static IServiceCollection HealthCheckServiceRegistration(this IServiceCollection services,IConfiguration configuration)
        {
            services.HealtCheckExtension(configuration);

            return services;
        }

        public static WebApplication HealthCheckWebApplicationRegistration(this WebApplication app,IConfiguration configuration)
        {
            app.HealtCheckApp();

            return app;
        }
    }
}
