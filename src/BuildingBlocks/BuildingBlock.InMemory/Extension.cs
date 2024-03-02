using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlock.InMemory
{
    public static class Extension
    {
        public static IServiceCollection InMemoryDbContext<T,TId>(this IServiceCollection services, IConfiguration configuration)

        {
            services.AddScoped(typeof(IInMemoryRepository<>), typeof(InMemoryRepository<>));
            services.AddScoped(typeof(IInMemoryRepository<,>), typeof(InMemoryRepository<,>));

            return services;
        }
    }
}
