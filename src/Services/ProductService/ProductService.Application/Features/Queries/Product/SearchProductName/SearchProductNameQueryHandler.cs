using BuildingBlock.Base.Abstractions;
using MediatR;
using ProductService.Domain.Models;
using ProductService.Domain.Constants;

namespace ProductService.Application.Features.Queries.Product.SearchProductName
{
    public class SearchProductNameQueryHandler : IRequestHandler<SearchProductNameQueryRequest, SearchProductNameQueryResponse>
    {
        private readonly ICompleteService<ProductElasticModel> _completeProductService;

        public SearchProductNameQueryHandler(ICompleteService<ProductElasticModel> completeProductService)
        {
            _completeProductService = completeProductService;
        }

        public async Task<SearchProductNameQueryResponse> Handle(SearchProductNameQueryRequest request, CancellationToken cancellationToken)
        {
            var productElasticModels = await _completeProductService.AutoAnalyzeWithLike(Constant.Fields.ProductName, request.word);

            return new(productElasticModels);
        }
    }
}
