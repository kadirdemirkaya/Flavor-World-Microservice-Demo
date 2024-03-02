using ImageService.Infrastructure.Persistence.Data;
using ImageService.Infrastructure.Persistence.Seeds;
using ImageService.Infrastructure.Registrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection ImageInfrastructureServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.DatabaseServiceRegistration(configuration);

            services.IUnitOfWorkRegistration(configuration);

            services.ServiceRegsitration(configuration);

            services.JsonWebTokenServiceRegistration(configuration);

            services.GrpcRegistration();

            return services;
        }

        public static WebApplicationBuilder ImageInfrastructureBuilderInjection(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.MsSqlServiceBuilder(configuration);

            return builder;
        }

        public static WebApplication ImageInfrastructureApplicationInjection(this WebApplication app)
        {
            app.MiddlewareRegistrationApp();

            app.HostSettingRegistration<ImageDbContext>(async (context, serviceProvider) =>
            {
                var services = app.Services;

                var dbContextSeed = new ImageDbContextSeed();
                dbContextSeed.SeedAsync(context).GetAwaiter();
            });

            app.GrpcRegistrationApp();

            return app;
        }
    }
}
