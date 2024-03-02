using Consul;
using ProductService.Application.Configurations;
using Serilog;

namespace ProductService.Api.Registrations
{
    public static class Consul
    {
        public static IServiceCollection ConsulRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = GetConfigs.GetConsulConnectionString;
                consulConfig.Address = new Uri(address);
            }));

            return services;
        }

        public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

            Log.Information("Registering with Consul");
            consulClient.Agent.ServiceDeregister(GetConfigs.GetAgentServiceRegistration.ID).Wait();
            consulClient.Agent.ServiceRegister(GetConfigs.GetAgentServiceRegistration).Wait();

            lifetime.ApplicationStopped.Register(() =>
            {
                Log.Information("Deregistering from Consul");
                consulClient.Agent.ServiceDeregister(GetConfigs.GetAgentServiceRegistration.ID);
            });

            return app;
        }
    }
}
