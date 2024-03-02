using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BuildingBlock.Redis
{
    public static class Extension
    {
        public static IServiceCollection RedisServiceExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRedisService<>), typeof(RedisService<>));

            services.AddScoped(typeof(IRedisService<,>), typeof(RedisService<,>));

            services.AddScoped(typeof(IRedisRepository<>), typeof(RedisRepository<>));

            return services;
        }
    }
}
