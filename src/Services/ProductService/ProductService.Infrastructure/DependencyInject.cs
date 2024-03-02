using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Infrastructure.Registrations;

namespace ProductService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection ProductInfrastructureServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {

            services.DatabaseServiceRegistration(configuration);

            if (bool.Parse(configuration["DbState"]) == true)
                services.InMemoryServiceRegistration(configuration);

            services.MediatrRegistrationService(configuration);

            services.EventServiceRegistration(configuration);

            services.RedisRegistration(configuration);

            services.ServiceRegistration();

            services.UnitOfWorkRegistration(configuration);

            services.JsonWebTokenServiceRegistration(configuration);

            //services.RabbitMqRegistration(configuration);

            services.ElasticRegistration(configuration);

            services.LogRegistrationService(configuration);

            return services;
        }

        public static WebApplicationBuilder ProductInfrastructureBuilderInjection(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.LogRegistrationBuilder(configuration);

            return builder;
        }

        public static WebApplication ProductInfrastructureApplicationInjection(this WebApplication app, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            app.MiddlewareRegistrationApp();

            //app.RabbitMqRegistrationApp(serviceProvider);

            app.RedisRegistrationApp(serviceProvider);

            return app;
        }
    }
}
