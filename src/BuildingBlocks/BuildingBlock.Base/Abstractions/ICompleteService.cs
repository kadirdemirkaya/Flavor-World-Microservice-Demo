using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Models.Base;
using Nest;

namespace BuildingBlock.Base.Abstractions
{
    public interface ICompleteService<T>
        where T : ElasticModel
    {
        public Task<List<T>> AutoComplete(string field, string query, bool transport = false);

        public Task<List<T>> AutoCompleteInBetween(string field, string query);

        public Task<List<T>> AutoMatchInBetween(string field, string query);

        public Task<List<T>> AutoMatchWithoutSensitive(string field, string query);

        public Task<List<T>> AutoAnalyzeWithLike(string field, string query);
    }
}
