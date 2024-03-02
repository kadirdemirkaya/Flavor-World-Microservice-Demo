using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Models.Base;
using BuildingBlock.Redis;

namespace BuildingBlock.Factory.Factories
{
    public static class RedisFactory<T, TId>
         where T : Entity<TId>
        where TId : ValueObject
    {
        public static IRedisService<T, TId> CreateForEntity(RedisConfig redisConfig, IServiceProvider sp)
            => new RedisService<T, TId>(redisConfig, sp);
    }

    public static class RedisFactory<T>
          where T : class
    {
        public static IRedisService<T> CreateForEntity(RedisConfig redisConfig, IServiceProvider sp)
            => new RedisService<T>(redisConfig, sp);
    }

    public static class RedisRepositoryFactory<T>
        where T : RedisModel
    {
        public static IRedisRepository<T> CreateRepository(InMemoryConfig memoryConfig, IServiceProvider serviceProvider)
            => new RedisRepository<T>(memoryConfig, serviceProvider);
    }
}
