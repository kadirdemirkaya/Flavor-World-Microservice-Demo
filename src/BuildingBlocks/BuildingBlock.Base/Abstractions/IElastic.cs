using BuildingBlock.Base.Models.Base;

namespace BuildingBlock.Base.Abstractions
{
    public interface IElastic<T, TId> : IBaseSearch<T, TId>
         where T : Entity<TId>
         where TId : ValueObject
    {

    }
    public interface IElastic<T> : IBaseSearch<T>
        where T : class
    {

    }
}
