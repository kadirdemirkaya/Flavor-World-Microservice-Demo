using BasketService.Api.Registrations;

namespace BasketService.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection BasketApiServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConsulRegistration(configuration);

            services.CorsRegistration();

            services.SessionRegistration();

            services.SwaggerRegistrationService(configuration);

            services.ValidationRegistration(configuration);

            services.LogRegistrationService(configuration);

            services.HealthCheckServiceRegistration(configuration);

            return services;
        }

        public static WebApplicationBuilder BasketApiBuilderInjection(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.LogApiRegistrationBuilder(configuration);

            return builder;
        }

        public static WebApplication BasketApiApplicationInjection(this WebApplication app, IHostApplicationLifetime lifetime)
        {
            app.RegisterWithConsul(lifetime);

            app.CorsRegistrationApp();

            app.SessionRegistrationApp();

            app.SwaggerRegistrationApp();

            app.HealthCheckWebApplicationRegistration();

            return app;
        }
    }
}
