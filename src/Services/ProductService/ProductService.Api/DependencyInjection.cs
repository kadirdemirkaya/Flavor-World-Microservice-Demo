using ProductService.Api.Registrations;

namespace ProductService.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection ProductApiServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConsulRegistration(configuration);

            services.CorsRegistration();

            services.SessionRegistration();

            services.SwaggerRegistrationService(configuration);

            services.HealthCheckRegistration(configuration);

            return services;
        }

        public static WebApplication ProductApiApplicationInjection(this WebApplication app, IHostApplicationLifetime lifetime)
        {
            app.RegisterWithConsul(lifetime);

            app.SwaggerRegistrationApp();

            app.CorsRegistrationApp();

            app.SessionRegistrationApp();

            app.HealthCheckRegistrationApp();

            return app;
        }
    }
}
