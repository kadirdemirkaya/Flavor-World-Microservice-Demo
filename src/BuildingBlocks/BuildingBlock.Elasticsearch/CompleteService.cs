using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Models.Base;
using Nest;
using Newtonsoft.Json;

namespace BuildingBlock.Elasticsearch
{
    public class CompleteService<T> : ICompleteService<T>
        where T : ElasticModel
    {
        public readonly IServiceProvider _serviceProvider;

        private readonly IElasticClient _elasticClient;

        private static string IndexName = typeof(T).Name.ToLower();
        protected SearchConfig SearchConfig { get; private set; }

        public CompleteService(IServiceProvider serviceProvider, SearchConfig config)
        {
            _serviceProvider = serviceProvider;
            SearchConfig = config;
            if (config.Connection != null)
            {
                var connJson = JsonConvert.SerializeObject(config.Connection, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });

                _elasticClient = new ElasticClient(ElasticSetting(JsonConvert.DeserializeObject<string>(connJson)));
            }
            IndexName = GetIndexName<T>();
            CreateIndexWithNameAsync().GetAwaiter().GetResult();
        }

        public ConnectionSettings ElasticSetting(string connection) => new ConnectionSettings(new Uri(connection));

        private string GetIndexName<T>() => typeof(T).Name.ToLower();

        public async Task CreateIndexWithNameAsync()
        {
            await CreateIndexAsync();
        }

        public async Task<bool> CreateIndexAsync()
        {
            try
            {
                if (!(await _elasticClient.Indices.ExistsAsync(IndexName)).Exists)
                {
                    await _elasticClient.Indices.CreateAsync(IndexName, c =>
                    {
                        c.Map<T>(p => p.AutoMap());
                        return c;
                    });
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Serilog.Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }
        }



        public async Task<List<T>> AutoComplete(string field, string query, bool transport = false)
        {
            try
            {
                if (transport)
                {
                    var responseT = await _elasticClient.SearchAsync<T>(s => s
                            .Index(IndexName)
                            .Query(q => q
                            .Fuzzy(fz => fz.Field(field.ToLower())
                            .Value(query.ToLower()).Transpositions(true))
                    ));
                    if (responseT.Documents is null)
                        return null;
                    return responseT.Documents.ToList();
                }
                var response = await _elasticClient.SearchAsync<T>(s => s
                       .Index(IndexName)
                       .Query(q => q
                       .Fuzzy(fz => fz.Field(field.ToLower())
                       .Value(query.ToLower()).Fuzziness(Fuzziness.EditDistance(4))
                    )
                ));
                if (response.Documents is null)
                    return null;
                return response.Documents.ToList();
            }
            catch (System.Exception ex)
            {
                Serilog.Log.Error("Elasticsearch Error : " + ex.Message);
                return null;
            }
        }

        public async Task<List<T>> AutoCompleteInBetween(string field, string query)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                        .From(0)
                        .Take(10)
                        .Index(IndexName)
                        .Query(q => q
                        .Bool(b => b
                        .Should(m => m
                        .Wildcard(w => w
                        .Field(field.ToLower())
                        .Value(query.ToLower() + "*"))))));
                if (response.Documents is null)
                    return null;
                return response.Documents.ToList();
            }
            catch (System.Exception ex)
            {
                Serilog.Log.Error("Elasticsearch Error : " + ex.Message);
                return null;
            }
        }

        public async Task<List<T>> AutoMatchInBetween(string field, string query)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                    .Index(IndexName)
                    .Query(q => q.MatchPhrasePrefix(m => m.Field(field.ToLower()).Query(query.ToLower()).MaxExpansions(10)))
                );
                if (response.Documents is null)
                    return null;
                return response.Documents.ToList();
            }
            catch (System.Exception ex)
            {
                Serilog.Log.Error("Elasticsearch Error : " + ex.Message);
                return null;
            }
        }

        public async Task<List<T>> AutoMatchWithoutSensitive(string field, string query)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                                .Index(IndexName)
                            .Query(q => q
                        .MultiMatch(mm => mm
                        .Fields(f => f.Field(field.ToLower()))
                        .Type(TextQueryType.PhrasePrefix)
                        .Query(query.ToLower())
                        .MaxExpansions(10)
                    )));
                if (response.Documents is null)
                    return null;
                return response.Documents.ToList();
            }
            catch (System.Exception ex)
            {
                Serilog.Log.Error("Elasticsearch Error : " + ex.Message);
                return null;
            }
        }


        public async Task<List<T>> AutoAnalyzeWithLike(string field, string query)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                                            .Index(IndexName)
                                        .Query(q => q
                                  .QueryString(qs => qs
                                .AnalyzeWildcard()
                            .Query("*" + query.ToLower() + "*")
                        .Fields(fs => fs.Fields(field.ToLower())
                    ))));
                if (response.Documents is null)
                    return null;
                return response.Documents.ToList();
            }
            catch (System.Exception ex)
            {
                Serilog.Log.Error("Elasticsearch Error : " + ex.Message);
                return null;
            }
        }
    }
}
