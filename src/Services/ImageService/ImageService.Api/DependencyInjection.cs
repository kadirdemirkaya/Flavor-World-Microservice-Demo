using ImageService.Api.Registrations;
using ImageService.Infrastructure;
using System.Reflection;

namespace ImageService.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection ImageApiServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConsulRegistration(configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.CorsRegistration();

            services.SessionRegistration();

            services.LogRegistrationService(configuration);

            services.SwaggerRegistrationService(configuration);

            services.ValidationRegistration(configuration);

            services.HealthCheckServiceRegistration(configuration);

            return services;
        }

        public static WebApplicationBuilder ImageApiBuilderInjection(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.LogApiRegistrationBuilder(configuration);

            return builder;
        }

        public static WebApplication ImageApiApplicationInjection(this WebApplication app, IHostApplicationLifetime lifetime)
        {
            app.RegisterWithConsulApp(lifetime);

            app.CorsRegistrationApp();

            app.SessionRegistrationApp();

            app.SwaggerRegistrationApp();

            app.HealthCheckWebApplicationRegistration();

            return app;
        }
    }
}
