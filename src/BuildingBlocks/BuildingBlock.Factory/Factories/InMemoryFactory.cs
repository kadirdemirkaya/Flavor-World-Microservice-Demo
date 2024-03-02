using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using BuildingBlock.Base.Models.Base;
using BuildingBlock.InMemory;
using BuildingBlock.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlock.Factory.Factories
{
    public static class InMemoryFactory<T, TId>
         where T : Entity<TId>
         where TId : ValueObject
    {
        public static IInMemoryRepository<T, TId> CreateForEntity(InMemoryConfig inMemoryConfig, DbContext dbContext, IServiceProvider serviceProvider)
        {
            return inMemoryConfig.InMemoryType switch
            {
                InMemoryType.Memory => new InMemoryRepository<T, TId>(inMemoryConfig, dbContext, serviceProvider),
                _ => new InMemoryRepository<T, TId>(inMemoryConfig, dbContext, serviceProvider)
            };
        }

        public static IRedisService<T, TId> CreateForEntity(InMemoryConfig inMemoryConfig, IServiceProvider serviceProvider)
        {
            return inMemoryConfig.InMemoryType switch
            {
                InMemoryType.Memory => new RedisService<T, TId>(inMemoryConfig, serviceProvider),
                _ => new RedisService<T, TId>(inMemoryConfig, serviceProvider)
            };
        }
    }
    public static class InMemoryFactory<T>
        where T : class
    {
        public static IInMemoryRepository<T> CreateForEntity(InMemoryConfig inMemoryConfig, DbContext dbContext, IServiceProvider serviceProvider)
        {
            return inMemoryConfig.InMemoryType switch
            {
                InMemoryType.Memory => new InMemoryRepository<T>(inMemoryConfig, dbContext, serviceProvider),
                _ => new InMemoryRepository<T>(inMemoryConfig, dbContext, serviceProvider)
            };
        }
        public static IRedisService<T> CreateForEntity(InMemoryConfig inMemoryConfig, IServiceProvider serviceProvider)
        {
            return inMemoryConfig.InMemoryType switch
            {
                InMemoryType.Memory => new RedisService<T>(inMemoryConfig, serviceProvider),
                _ => new RedisService<T>(inMemoryConfig, serviceProvider)
            };
        }
    }
}
