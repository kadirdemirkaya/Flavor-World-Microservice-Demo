using BuildingBlock.Base.Enums;

namespace BuildingBlock.Base.Configs
{
    public class SearchConfig
    {
        public object Connection { get; set; }

        public SearchType SearchBaseType { get; set; } = SearchType.ElasticSearch;
    }
}
