using BuildingBlock.Swagger;

namespace OrderService.Api.Registrations
{
    public static class Swagger
    {
        public static IServiceCollection SwaggerRegistrationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.SwaggerServiceRegistration(configuration);

            return services;
        }

        public static WebApplication SwaggerRegistrationApp(this WebApplication app, IConfiguration configuration)
        {
            app.SwaggerApplicationRegistration(configuration);

            return app;
        }
    }
}
