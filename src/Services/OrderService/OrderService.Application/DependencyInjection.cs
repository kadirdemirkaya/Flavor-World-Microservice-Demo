using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Registrations;

namespace OrderService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection OrderApplicationServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.LogRegistrationService(configuration);

            services.MapperRegistrationService();

            services.MediatrRegistrationService(configuration);

            return services;
        }

        public static WebApplicationBuilder OrderApplicationBuilderInjection(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.LogRegistrationBuilder(configuration);

            return builder;
        }
    }
}
