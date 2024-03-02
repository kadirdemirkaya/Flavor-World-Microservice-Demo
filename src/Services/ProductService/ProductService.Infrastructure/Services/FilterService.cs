using Microsoft.Extensions.Configuration;
using Nest;
using ProductService.Application.Abstractions;
using ProductService.Domain.Aggregate.ProductAggregate;
using ProductService.Domain.Aggregate.ProductAggregate.Enums;
using ProductService.Domain.Models;

namespace ProductService.Infrastructure.Services
{
    public class FilterService : IFilterService
    {
        private readonly IConfiguration _configuration;

        private readonly IElasticClient _elasticClient;

        public FilterService(IConfiguration configuration)
        {
            _configuration = configuration;
            _elasticClient = new ElasticClient(ElasticSetting());
        }

        public ConnectionSettings ElasticSetting() => new ConnectionSettings(new Uri(_configuration["ElasticSearch:Uri"]));

        public async Task<List<ProductElasticModel>> CategoryFilterAsync(string indexName, ProductCategory category)
        {
            var response = await _elasticClient.SearchAsync<ProductElasticModel>(s => s
                .Index(indexName.ToLower())
                .Query(q => q
                    .Bool(b => b
                        .Must(
                            st => st.Match(m => m
                                .Field(f => f.ProductCategory)
                                .Query(((int)category).ToString())
                            )
                        )
                    )
                )
            );
            if (response.IsValid)
                return response.Documents.ToList();
            return default;
        }
    }
}
