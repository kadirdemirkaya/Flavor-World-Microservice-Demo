using BuildingBlock.HealthCheck;
using OrderService.Api.Registrations;

namespace OrderService.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection OrderApiServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConsulRegistration(configuration);

            services.CorsRegistration();

            services.SwaggerRegistrationService(configuration);

            services.SessionRegistration();

            services.ValidationRegistration(configuration);

            services.HealthCheckServiceRegistration(configuration);

            return services;
        }

        public static WebApplication OrderApiApplicationInjection(this WebApplication app, IConfiguration configuration, IHostApplicationLifetime lifetime)
        {
            app.RegisterWithConsul(lifetime);

            app.CorsRegistrationApp();

            app.SwaggerRegistrationApp(configuration);

            app.SessionRegistrationApp();

            app.HealthCheckWebApplicationRegistration(configuration);

            return app;
        }
    }
}
