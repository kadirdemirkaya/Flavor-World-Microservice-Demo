using AuthenticationService.Application.Abstractions;
using AuthenticationService.Infrastructure.Persistence.Data;
using AuthenticationService.Infrastructure.Persistence.Seeds;
using AuthenticationService.Infrastructure.Registrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AuthenticationInfrastructureServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.MapperRegistrationService();

            services.DatabaseServiceRegistration(configuration);

            services.JsonWebTokenServiceRegistration(configuration);

            //services.InMemoryServiceRegistration(configuration);

            services.MediatrRegistrationService(configuration);

            services.EventServiceRegistration(configuration);

            services.IUnitOfWorkRegistration(configuration);

            services.ServiceRegsitration(configuration);

            services.GrpcRegistration();

            services.CookieRegistration();

            return services;
        }

        public static WebApplication AuthenticationInfrastructureApplicationInjection(this WebApplication app)
        {
            app.MiddlewareRegistrationApp();

            app.HostSettingRegistration<AuthenticationDbContext>(async (context, serviceProvider) =>
            {
                var services = app.Services;

                var hashService = services.GetRequiredService<IHashService>();

                var dbContextSeed = new AuthenticationDbContextSeed(hashService);
                dbContextSeed.SeedAsync(context).GetAwaiter();
            });

            app.GrpcRegistrationApp();

            return app;
        }
    }
}
