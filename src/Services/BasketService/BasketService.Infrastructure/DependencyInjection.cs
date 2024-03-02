using BasketService.Infrastructure.Registrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection BasketInfrastructureServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.MediatrRegistration(configuration);

            services.DatabaseServiceRegistration(configuration);

            if (bool.Parse(configuration["InMemory"]) == true)
                services.InMemoryServiceRegistration(configuration);

            services.RedisRegistration(configuration);

            services.JsonWebTokenServiceRegistration(configuration);

            services.EventServiceRegistration(configuration);

            services.IUnitOfWorkRegistration(configuration);

            services.ServiceRegsitration(configuration);

            //services.RabbitMqRegistration(configuration);

            return services;
        }

        public static WebApplication BasketInfrastructureApplicationInjection(this WebApplication app, IServiceProvider serviceProvider)
        {
            //app.RabbitMqRegistrationApp(serviceProvider);

            app.RedisRegistrationApp(serviceProvider);

            return app;
        }
    }
}
