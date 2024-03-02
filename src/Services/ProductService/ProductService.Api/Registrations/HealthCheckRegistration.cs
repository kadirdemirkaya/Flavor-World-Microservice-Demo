using BuildingBlock.HealthCheck;

namespace ProductService.Api.Registrations
{
    public static class HealthCheck
    {
        public static IServiceCollection HealthCheckRegistration(this IServiceCollection services,IConfiguration configuration)
        {
            services.HealtCheckExtension(configuration);

            return services;
        }

        public static WebApplication HealthCheckRegistrationApp(this WebApplication app)
        {
            app.HealtCheckApp();

            return app;
        }
    }
}
