using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using BuildingBlock.Base.Models.Base;
using BuildingBlock.Elasticsearch;

namespace BuildingBlock.Factory.Factories
{
    public static class ElasticFactory<T, TId>
          where T : Entity<TId>
          where TId : ValueObject
    {
        public static IElastic<T, TId> CreateForEntity(SearchConfig searchConfig, IServiceProvider sp)
        {
            return searchConfig.SearchBaseType switch
            {
                SearchType.ElasticSearch => new ElasticService<T, TId>(searchConfig, sp),
                _ => new ElasticService<T, TId>(searchConfig, sp)
            };
        }
    }

    public static class ElasticFactory<T>
        where T : ElasticModel
    {
        public static IElastic<T> CreateForClass(SearchConfig searchConfig, IServiceProvider sp)
        {
            return searchConfig.SearchBaseType switch
            {
                SearchType.ElasticSearch => new ElasticService<T>(searchConfig, sp),
                _ => new ElasticService<T>(searchConfig, sp)
            };
        }

        public static ICompleteService<T> CreateForComplete(SearchConfig searchConfig, IServiceProvider sp)
        {
            return searchConfig.SearchBaseType switch
            {
                SearchType.ElasticSearch => new CompleteService<T>(sp, searchConfig),
                _ => new CompleteService<T>(sp, searchConfig)
            };
        }
    }
}
