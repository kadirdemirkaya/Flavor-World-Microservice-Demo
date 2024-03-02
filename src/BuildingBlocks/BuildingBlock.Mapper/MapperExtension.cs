using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlock.Mapper
{
    public static class Extension
    {
        public static IServiceCollection MapperExtension(this IServiceCollection services, Assembly assembly)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(assembly);
            });

            return services;
        }
    }
}
