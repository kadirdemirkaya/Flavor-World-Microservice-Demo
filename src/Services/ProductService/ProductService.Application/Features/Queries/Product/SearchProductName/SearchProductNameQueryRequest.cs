using MediatR;

namespace ProductService.Application.Features.Queries.Product.SearchProductName
{
    public record SearchProductNameQueryRequest(
        string word
    ) : IRequest<SearchProductNameQueryResponse>;
}
