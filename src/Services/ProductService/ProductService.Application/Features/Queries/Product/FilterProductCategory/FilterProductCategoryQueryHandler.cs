using AutoMapper;
using MediatR;
using ProductService.Application.Abstractions;
using ProductService.Domain.Constants;
using ProductService.Domain.Models;

namespace ProductService.Application.Features.Queries.Product.FilterProductCategory
{
    public class FilterProductCategoryQueryHandler : IRequestHandler<FilterProductCategoryQueryRequest, FilterProductCategoryQueryResponse>
    {
        private readonly IFilterService _filterService;
        public FilterProductCategoryQueryHandler(IFilterService filterService)
        {
            _filterService = filterService;
        }

        public async Task<FilterProductCategoryQueryResponse> Handle(FilterProductCategoryQueryRequest request, CancellationToken cancellationToken)
        {
            var filterList = await _filterService.CategoryFilterAsync(nameof(ProductElasticModel).ToLower(), request.ProductCategory);
            return new(filterList);
        }
    }
}
