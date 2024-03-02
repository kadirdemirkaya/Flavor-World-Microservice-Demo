using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Models.Base;
using Nest;
using Newtonsoft.Json;
using Serilog;

namespace BuildingBlock.Elasticsearch
{
    public class ElasticService<T, TId> : IElastic<T, TId>
        where T : Entity<TId>
        where TId : ValueObject
    {
        public readonly IServiceProvider ServiceProvider;
        protected SearchConfig SearchConfig { get; private set; }

        private readonly IElasticClient _elasticClient;

        private readonly string IndexName;
        private ElasticPersistenceConnection elasticPersistenceConnection;

        public ElasticService(SearchConfig config, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            if (config.Connection != null)
            {
                var connJson = JsonConvert.SerializeObject(config.Connection, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });

                _elasticClient = new ElasticClient(ElasticSetting(JsonConvert.DeserializeObject<string>(connJson)));

                elasticPersistenceConnection = new ElasticPersistenceConnection(config);
            }
            IndexName = GetIndexName<T>();
            CreateIndexWithNameAsync().GetAwaiter().GetResult();
        }

        public ElasticService(SearchConfig config)
        {
            if (config.Connection != null)
            {
                elasticPersistenceConnection = new ElasticPersistenceConnection(config);
                _elasticClient = new ElasticClient(elasticPersistenceConnection.GetConnection());
            }
            IndexName = GetIndexName<T>();
            CreateIndexWithNameAsync().GetAwaiter().GetResult();
        }

        public ConnectionSettings ElasticSetting(string connection) => new ConnectionSettings(new Uri(connection));
        private string GetIndexName<T>() => typeof(T).Name.ToLower();

        public async Task<bool> ChekIndexAsync(string indexName)
        {
            try
            {
                var anyy = await _elasticClient.Indices.ExistsAsync(indexName);
                if (anyy.Exists)
                    return true;
                return false;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }
        }

        public async Task<T> GetAsync(string id)
        {
            try
            {
                var response = await _elasticClient.GetAsync(DocumentPath<T>.Id(id).Index(IndexName));
                if (response.IsValid)
                    return response.Source;
                Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                return null;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null;
            }
        }

        public async Task<T> GetAsync(IGetRequest request)
        {
            try
            {
                var response = await _elasticClient.GetAsync<T>(request);
                if (response.IsValid)
                    return response.Source;
                Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                return null;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null;
            }

        }

        public async Task<T> FindAsync(string id)
        {
            try
            {
                var response = await _elasticClient.GetAsync(DocumentPath<T>.Id(id).Index(IndexName));

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return null;
                }

                return response.Source;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null;
            }

        }

        public async Task<T> FindAsync(IGetRequest request)
        {
            try
            {
                var response = await _elasticClient.GetAsync<T>(request);

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return null;
                }

                return response.Source;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null;
            }

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var search = new SearchDescriptor<T>(IndexName).MatchAll();
                var response = await _elasticClient.SearchAsync<T>(search);

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return null;
                }

                return response.Hits.Select(hit => hit.Source).ToList();
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null!;
            }
        }

        public async Task<IEnumerable<T>> GetManyAsync(IEnumerable<string> ids)
        {
            try
            {
                var response = await _elasticClient.GetManyAsync<T>(ids, IndexName);
                return response.Select(item => item.Source).ToList();
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null!;
            }

        }

        public async Task<IEnumerable<T>> SearchAsync(Func<QueryContainerDescriptor<T>, QueryContainer> request)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s =>
                                        s.Index(IndexName)
                                        .Query(request));

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return null;
                }

                return response.Hits.Select(hit => hit.Source).ToList();
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null!;
            }

        }

        public async Task<ISearchResponse<T>> SearchAsync(Func<QueryContainerDescriptor<T>, QueryContainer> request,
            Func<AggregationContainerDescriptor<T>, IAggregationContainer> aggregationsSelector)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s =>
             s.Index(IndexName)
                 .Query(request)
                 .Aggregations(aggregationsSelector));

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return null;
                }

                return response;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null!;
            }

        }

        public async Task<IEnumerable<T>> SearchAsync(Func<SearchDescriptor<T>, ISearchRequest> selector)
        {
            try
            {
                var list = new List<T>();
                var response = await _elasticClient.SearchAsync(selector);

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return null;
                }

                return response.Hits.Select(hit => hit.Source).ToList();
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null!;
            }

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
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }
        }

        public async Task CreateIndexWithNameAsync()
        {
            await CreateIndexAsync();
        }

        public async Task<bool> CreateIndexAsync(string indexName)
        {
            try
            {
                if (!(await _elasticClient.Indices.ExistsAsync(indexName)).Exists)
                {
                    await _elasticClient.Indices.CreateAsync(indexName, c =>
                    {
                        c.Map<T>(p => p.AutoMap());
                        return c;
                    });
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }

        }

        public async Task<bool> DeleteIndexAsync(string indexName)
        {
            try
            {
                var response = await _elasticClient.Indices.DeleteAsync(indexName);
                if (response.IsValid)
                    return true;
                Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                return false;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }
        }

        public async Task<bool> CreateIndexWithMapAsync(string indexName, Func<TypeMappingDescriptor<object>, ITypeMapping> mappingDescriptor)
        {
            try
            {
                var anyy = await _elasticClient.Indices.ExistsAsync(indexName);
                if (anyy.Exists)
                    return false;

                var response = await _elasticClient.Indices.CreateAsync(indexName,
                    ci => ci
                        .Index(indexName)
                        .Map(mappingDescriptor)
                        .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                        );
                if (response.IsValid)
                    return true;
                Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                return false;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }

        }

        public async Task<bool> InsertAsync(T model)
        {
            try
            {
                var response = await _elasticClient.IndexAsync(model, descriptor => descriptor.Index(IndexName));

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return false;
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }

        }

        public async Task<bool> InsertManyAsync(IList<T> models)
        {
            try
            {
                await CreateIndexAsync();
                var response = await _elasticClient.IndexManyAsync(models, IndexName);

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return false;
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }

        }

        public async Task<bool> UpdateAsync(T model)  /*!!!!!*/
        {
            try
            {
                var response = await _elasticClient.UpdateAsync(DocumentPath<T>.Id(model.Id.ToString()).Index(IndexName), p => p.Doc(model));

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }

        }

        public async Task<bool> UpdatePartAsync(T model, object partialEntity)
        {
            try
            {
                var request = new UpdateRequest<T, object>(IndexName, model.Id.ToString()) /* !!!!!!!!!!!!!*/
                {
                    Doc = partialEntity
                };
                var response = await _elasticClient.UpdateAsync(request);

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return false;
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            try
            {
                var response = await _elasticClient.DeleteAsync(DocumentPath<T>.Id(id).Index(IndexName));

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }

        }

        public async Task<bool> DeleteByQueryAsync(Func<QueryContainerDescriptor<T>, QueryContainer> selector)
        {
            try
            {
                var response = await _elasticClient.DeleteByQueryAsync<T>(q => q
               .Query(selector)
               .Index(IndexName)
                );

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }

        }

        public async Task<long> GetTotalCountAsync()
        {
            try
            {
                var search = new SearchDescriptor<T>(IndexName).MatchAll();
                var response = await _elasticClient.SearchAsync<T>(search);

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return default;
                }

                return response.Total;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return default;
            }

        }

        public async Task<bool> ExistAsync(string id)
        {
            try
            {
                var response = await _elasticClient.DocumentExistsAsync(DocumentPath<T>.Id(id).Index(IndexName));

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return false;
                }

                return response.Exists;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }
        }

        public async Task<bool> HealtCheck()
        {
            var healt = await _elasticClient.PingAsync();
            return healt.IsValid;
        }
    }
    public class ElasticService<T> : IElastic<T> where T : ElasticModel
    {
        public readonly IServiceProvider ServiceProvider;
        protected SearchConfig SearchConfig { get; private set; }

        private readonly IElasticClient _elasticClient;

        private readonly string IndexName;

        public ElasticService(SearchConfig config, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
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

        public async Task<bool> ChekIndexAsync(string indexName)
        {
            try
            {
                var anyy = await _elasticClient.Indices.ExistsAsync(indexName);
                if (anyy.Exists)
                    return true;
                return false;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }
        }

        public async Task<T> GetAsync(string id)
        {
            try
            {
                var response = await _elasticClient.GetAsync(DocumentPath<T>.Id(id).Index(IndexName));
                if (response.IsValid)
                    return response.Source;
                Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                return null;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null;
            }
        }

        public async Task<T> GetAsync(IGetRequest request)
        {
            try
            {
                var response = await _elasticClient.GetAsync<T>(request);
                if (response.IsValid)
                    return response.Source;
                Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                return null;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null;
            }

        }

        public async Task<T> FindAsync(string id)
        {
            try
            {
                var response = await _elasticClient.GetAsync(DocumentPath<T>.Id(id).Index(IndexName));

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return null;
                }

                return response.Source;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null;
            }

        }

        public async Task<T> FindAsync(IGetRequest request)
        {
            try
            {
                var response = await _elasticClient.GetAsync<T>(request);

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return null;
                }

                return response.Source;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null;
            }

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var search = new SearchDescriptor<T>(IndexName).MatchAll();
                var response = await _elasticClient.SearchAsync<T>(search);

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return null;
                }

                return response.Hits.Select(hit => hit.Source).ToList();
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null!;
            }
        }

        public async Task<IEnumerable<T>> GetManyAsync(IEnumerable<string> ids)
        {
            try
            {
                var response = await _elasticClient.GetManyAsync<T>(ids, IndexName);
                return response.Select(item => item.Source).ToList();
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null!;
            }

        }

        public async Task<IEnumerable<T>> SearchAsync(Func<QueryContainerDescriptor<T>, QueryContainer> request)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s =>
                                        s.Index(IndexName)
                                        .Query(request));

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return null;
                }

                return response.Hits.Select(hit => hit.Source).ToList();
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null!;
            }

        }

        public async Task<ISearchResponse<T>> SearchAsync(Func<QueryContainerDescriptor<T>, QueryContainer> request,
            Func<AggregationContainerDescriptor<T>, IAggregationContainer> aggregationsSelector)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s =>
             s.Index(IndexName)
                 .Query(request)
                 .Aggregations(aggregationsSelector));

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return null;
                }

                return response;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null!;
            }

        }

        public async Task<IEnumerable<T>> SearchAsync(Func<SearchDescriptor<T>, ISearchRequest> selector)
        {
            try
            {
                var list = new List<T>();
                var response = await _elasticClient.SearchAsync(selector);

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return null;
                }

                return response.Hits.Select(hit => hit.Source).ToList();
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return null!;
            }

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
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }
        }

        public async Task CreateIndexWithNameAsync()
        {
            await CreateIndexAsync();
        }

        public async Task<bool> CreateIndexAsync(string indexName)
        {
            try
            {
                if (!(await _elasticClient.Indices.ExistsAsync(indexName)).Exists)
                {
                    await _elasticClient.Indices.CreateAsync(indexName, c =>
                    {
                        c.Map<T>(p => p.AutoMap());
                        return c;
                    });
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }

        }

        public async Task<bool> DeleteIndexAsync(string indexName)
        {
            try
            {
                var response = await _elasticClient.Indices.DeleteAsync(indexName);
                if (response.IsValid)
                    return true;
                Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                return false;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }
        }

        public async Task<bool> CreateIndexWithMapAsync(string indexName, Func<TypeMappingDescriptor<object>, ITypeMapping> mappingDescriptor)
        {
            try
            {
                var anyy = await _elasticClient.Indices.ExistsAsync(indexName);
                if (anyy.Exists)
                    return false;

                var response = await _elasticClient.Indices.CreateAsync(indexName,
                    ci => ci
                        .Index(indexName)
                        .Map(mappingDescriptor)
                        .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                        );
                if (response.IsValid)
                    return true;
                Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                return false;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }

        }

        public async Task<bool> InsertAsync(T model)
        {
            try
            {
                var response = await _elasticClient.IndexAsync(model, descriptor => descriptor.Index(IndexName));

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return false;
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }

        }

        public async Task<bool> InsertManyAsync(IList<T> models)
        {
            try
            {
                await CreateIndexAsync();
                var response = await _elasticClient.IndexManyAsync(models, IndexName);

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return false;
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }

        }

        public async Task<bool> UpdateAsync(T model)
        {
            try
            {
                var response = await _elasticClient.UpdateAsync(DocumentPath<T>.Id(model.Id).Index(IndexName), p => p.Doc(model));

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }

        }

        public async Task<bool> UpdatePartAsync(T model, object partialEntity)
        {
            try
            {
                var request = new UpdateRequest<T, object>(IndexName, model.Id)
                {
                    Doc = partialEntity
                };
                var response = await _elasticClient.UpdateAsync(request);

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return false;
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            try
            {
                var response = await _elasticClient.DeleteAsync(DocumentPath<T>.Id(id).Index(IndexName));

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }

        }

        public async Task<bool> DeleteByQueryAsync(Func<QueryContainerDescriptor<T>, QueryContainer> selector)
        {
            try
            {
                var response = await _elasticClient.DeleteByQueryAsync<T>(q => q
               .Query(selector)
               .Index(IndexName)
                );

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }

        }

        public async Task<long> GetTotalCountAsync()
        {
            try
            {
                var search = new SearchDescriptor<T>(IndexName).MatchAll();
                var response = await _elasticClient.SearchAsync<T>(search);

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return default;
                }

                return response.Total;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return default;
            }

        }

        public async Task<bool> ExistAsync(string id)
        {
            try
            {
                var response = await _elasticClient.DocumentExistsAsync(DocumentPath<T>.Id(id).Index(IndexName));

                if (!response.IsValid)
                {
                    Log.Error(response.OriginalException, response.ServerError?.ToString()!);
                    return false;
                }

                return response.Exists;
            }
            catch (System.Exception ex)
            {
                Log.Error("ElasticSearch Error : " + ex.Message);
                return false;
            }
        }

        public async Task<bool> HealtCheck()
        {
            var healt = await _elasticClient.PingAsync();
            return healt.IsValid;
        }
    }
}
