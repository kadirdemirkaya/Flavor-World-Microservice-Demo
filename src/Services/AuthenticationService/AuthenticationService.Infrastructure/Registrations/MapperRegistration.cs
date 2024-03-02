using BuildingBlock.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure.Registrations
{
    public static class Mapper
    {
        public static IServiceCollection MapperRegistrationService(this IServiceCollection services)
        {
            services.MapperExtension(AssemblyReference.Assembly);

            return services;
        }
    }
}
