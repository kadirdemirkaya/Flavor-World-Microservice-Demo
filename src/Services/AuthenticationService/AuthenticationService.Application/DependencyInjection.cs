using AuthenticationService.Application.Registrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AuthenticationApplicationServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.MediatrRegistrationService(configuration);

            services.MapperRegistrationService();

            services.LogApplicationRegistrationService(configuration);

            return services;
        }

        public static WebApplicationBuilder AuthenticationApplicationBuilderInjection(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.LogApplicationRegistrationBuilder(configuration);

            return builder;
        }
    }
}
