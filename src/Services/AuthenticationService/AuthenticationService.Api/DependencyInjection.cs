using AuthenticationService.Api.Registration;

namespace AuthenticationService.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AuthenticationApiServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConsulRegistration(configuration);

            services.CorsRegistration();

            services.SessionRegistration(configuration);

            services.SwaggerRegistrationService(configuration);

            services.ValidationRegistration(configuration);

            services.LogRegistrationService(configuration);

            services.HealthCheckServiceRegistration(configuration);

            return services;
        }

        public static WebApplicationBuilder AuthenticationApiBuilderInjection(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.LogApiRegistrationBuilder(configuration);

            return builder;
        }

        public static WebApplication AuthenticationApiApplicationInjection(this WebApplication app, IHostApplicationLifetime lifetime)
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
