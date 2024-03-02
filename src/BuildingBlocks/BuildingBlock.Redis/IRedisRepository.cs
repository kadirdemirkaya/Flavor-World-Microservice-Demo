using BuildingBlock.Base.Enums;
using BuildingBlock.Base.Models.Base;

namespace BuildingBlock.Redis
{
    public interface IRedisRepository<T>
        where T : RedisModel
    {
        Task<bool> CreateAsync(string key, T value, RedisDataType dataType);
        bool Create(string key, T value, RedisDataType dataType);
        Task<T>? GetByIdAsync(string key, string? id, RedisDataType dataType);
        T? GetById(string key, string? id, RedisDataType dataType);
        IEnumerable<T?>? GetAll(string? key, RedisDataType dataType);
        bool Delete(string key, string? id, RedisDataType dataType);
        bool Update(string key, T value, RedisDataType dataType);
    }
}
