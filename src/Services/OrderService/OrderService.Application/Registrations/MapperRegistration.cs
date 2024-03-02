using Microsoft.Extensions.DependencyInjection;
using BuildingBlock.Mapper;

namespace OrderService.Application.Registrations
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
