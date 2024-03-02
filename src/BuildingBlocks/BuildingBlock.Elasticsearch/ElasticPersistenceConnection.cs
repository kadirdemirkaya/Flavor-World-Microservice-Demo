using BuildingBlock.Base.Configs;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using Polly;
using System.Net.Sockets;

namespace BuildingBlock.Elasticsearch
{
    public class ElasticPersistenceConnection : IDisposable
    {
        private IElasticClient _elasticClient;
        private object lock_object = new object();
        private readonly int RetryCount;
        private SearchConfig SearchConfig;
        private string connectionString;

        public ElasticPersistenceConnection(SearchConfig searchConfig, int retryCount = 5)
        {
            SearchConfig = searchConfig;
            if (SearchConfig.Connection != null)
            {
                var connJson = JsonConvert.SerializeObject(SearchConfig.Connection, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
                connectionString = connJson;
            }
            _elasticClient = new ElasticClient(ElasticDefaultSetting());
            RetryCount = retryCount;
        }

        public ElasticPersistenceConnection(string connectionStr, int retryCount = 5)
        {
            connectionString = connectionStr;
            _elasticClient = new ElasticClient(ElasticDefaultSetting());
            RetryCount = retryCount;
        }

        public IElasticClient ElasticClient
        {
            get
            {
                lock (lock_object)
                {
                    if (_elasticClient is null)
                        AssignReference(connectionString);
                    return _elasticClient;
                }
            }
        }

        public IElasticClient GetClient() => _elasticClient;

        public bool IsConnected => isConnection != null && isConnection is true;

        public ConnectionSettings? ElasticSetting(IConfiguration _configuration) => new ConnectionSettings(GetConnection(_configuration["ElasticSetting:ConnectionUrl"]!));

        public ConnectionSettings ElasticDefaultSetting() => new ConnectionSettings(GetConnection());

        public ConnectionSettings ElasticValueSetting(ElasticConnectionFactory connectionFactory) => new ConnectionSettings(GetConnection(connectionFactory));

        public bool isConnection => _elasticClient.Ping().IsValid;

        public Uri GetConnection(string connectionUrl)
        {
            return new Uri(connectionUrl);
        }

        public Uri GetConnection()
        {
            return new Uri($"{ElasticConnection.Host}.{ElasticConnection.Port}");
        }

        public Uri GetConnection(ElasticConnectionFactory elasticConnectionFactory)
        {
            return new Uri($"{elasticConnectionFactory.Host}.{elasticConnectionFactory.Port}");
        }

        public void AssignReference(string url) => _elasticClient = new ElasticClient(new Uri(url));

        public void Dispose()
        {
            _elasticClient = null;
        }

        public bool TryConnect()
        {
            lock (lock_object)
            {
                var policy = Polly.Policy.Handle<SocketException>()
                    .Or<UnexpectedElasticsearchClientException>()
                    .WaitAndRetry(RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {

                    }
                );

                policy.Execute(() =>
                {
                    _elasticClient = new ElasticClient(ElasticDefaultSetting());
                });

                if (IsConnected)
                    return true;
                return false;
            }
        }
    }
}
