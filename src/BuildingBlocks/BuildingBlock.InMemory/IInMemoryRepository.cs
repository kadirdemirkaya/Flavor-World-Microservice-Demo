using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models.Base;

namespace BuildingBlock.InMemory
{
    public interface IInMemoryRepository<T, TId> : ICaching<T, TId>, IWriteRepository<T, TId>, IReadRepository<T, TId>
       where T : Entity<TId>
       where TId : ValueObject
    {

    }

    public interface IInMemoryRepository<T> : ICaching<T>, IWriteRepository<T>, IReadRepository<T>
        where T : class
    {

    }
}
