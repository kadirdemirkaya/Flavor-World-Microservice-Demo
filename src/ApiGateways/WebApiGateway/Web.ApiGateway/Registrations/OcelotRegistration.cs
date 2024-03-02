using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace Web.ApiGateway.Registrations
{
    public static class Ocelot
    {
        public static IServiceCollection OcelotRegistration(this IServiceCollection services)
        {
            services.AddOcelot().AddConsul();

            return services;
        }

        public static WebApplication OcelotRegistrationApp(this WebApplication app)
        {
            app.UseOcelot().GetAwaiter().GetResult();

            return app;
        }
    }
}
