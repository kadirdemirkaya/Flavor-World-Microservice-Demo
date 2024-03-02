using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Infrastructure.Registrations;

namespace OrderService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection OrderInfrastructureServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            bool jsonValue = false;

            if (bool.TryParse(configuration["DbMemoryState"], out jsonValue))
            {
                services.InMemoryServiceRegistration(configuration);
            }

            services.MediatrRegistrationService(configuration);

            services.EventServiceRegistration(configuration);

            services.RedisRegistration(configuration);

            services.DatabaseServiceRegistration(configuration);

            services.UnitOfWorkRegistration(configuration);

            services.JsonWebTokenServiceRegistration(configuration);

            //services.RabbitMqRegistration(configuration);

            return services;
        }

        public static WebApplication OrderInfrastructureApplicationInjection(this WebApplication app, IServiceProvider serviceProvider)
        {
            //app.RabbitMqRegistrationApp(serviceProvider);

            app.RedisRegistrationApp(serviceProvider);

            return app;
        }
    }
}
