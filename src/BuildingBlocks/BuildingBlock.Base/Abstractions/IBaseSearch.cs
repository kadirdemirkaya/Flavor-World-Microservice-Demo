using BuildingBlock.Base.Models.Base;
using Nest;

namespace BuildingBlock.Base.Abstractions
{
    public interface IBaseSearch<T, TId>
        where T : Entity<TId>
        where TId : ValueObject
    {
        Task<bool> ChekIndexAsync(string indexName);
        Task<T> GetAsync(string id);
        Task<T> GetAsync(IGetRequest request);
        Task<T> FindAsync(string id);
        Task<T> FindAsync(IGetRequest request);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetManyAsync(IEnumerable<string> ids);
        Task<IEnumerable<T>> SearchAsync(Func<SearchDescriptor<T>, ISearchRequest> selector);
        Task<IEnumerable<T>> SearchAsync(Func<QueryContainerDescriptor<T>, QueryContainer> request);
        Task<ISearchResponse<T>> SearchAsync(Func<QueryContainerDescriptor<T>, QueryContainer> request, Func<AggregationContainerDescriptor<T>, IAggregationContainer> aggregationsSelector);
        Task<bool> CreateIndexAsync();
        Task<bool> CreateIndexAsync(string indexName);
        Task<bool> DeleteIndexAsync(string indexName);
        Task<bool> CreateIndexWithMapAsync(string indexName, Func<TypeMappingDescriptor<object>, ITypeMapping> mappingDescriptor);
        Task<bool> InsertAsync(T t);
        Task<bool> InsertManyAsync(IList<T> tList);
        Task<bool> UpdateAsync(T t);
        Task<bool> UpdatePartAsync(T t, object partialEntity);
        Task<long> GetTotalCountAsync();
        Task<bool> DeleteByIdAsync(string id);
        Task<bool> DeleteByQueryAsync(Func<QueryContainerDescriptor<T>, QueryContainer> selector);
        Task<bool> ExistAsync(string id);
        Task<bool> HealtCheck();
    }
    public interface IBaseSearch<T> where T : class
    {
        Task<bool> ChekIndexAsync(string indexName);
        Task<T> GetAsync(string id);
        Task<T> GetAsync(IGetRequest request);
        Task<T> FindAsync(string id);
        Task<T> FindAsync(IGetRequest request);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetManyAsync(IEnumerable<string> ids);
        Task<IEnumerable<T>> SearchAsync(Func<SearchDescriptor<T>, ISearchRequest> selector);
        Task<IEnumerable<T>> SearchAsync(Func<QueryContainerDescriptor<T>, QueryContainer> request);
        Task<ISearchResponse<T>> SearchAsync(Func<QueryContainerDescriptor<T>, QueryContainer> request, Func<AggregationContainerDescriptor<T>, IAggregationContainer> aggregationsSelector);
        Task<bool> CreateIndexAsync();
        Task<bool> CreateIndexAsync(string indexName);
        Task<bool> DeleteIndexAsync(string indexName);
        Task<bool> CreateIndexWithMapAsync(string indexName, Func<TypeMappingDescriptor<object>, ITypeMapping> mappingDescriptor);
        Task<bool> InsertAsync(T t);
        Task<bool> InsertManyAsync(IList<T> tList);
        Task<bool> UpdateAsync(T t);
        Task<bool> UpdatePartAsync(T t, object partialEntity);
        Task<long> GetTotalCountAsync();
        Task<bool> DeleteByIdAsync(string id);
        Task<bool> DeleteByQueryAsync(Func<QueryContainerDescriptor<T>, QueryContainer> selector);
        Task<bool> ExistAsync(string id);
        Task<bool> HealtCheck();
    }
}
