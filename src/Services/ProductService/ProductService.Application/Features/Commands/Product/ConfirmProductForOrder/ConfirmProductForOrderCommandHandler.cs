using BuildingBlock.Base.Abstractions;
using MediatR;
using ProductService.Domain.Aggregate.ProductAggregate.ValueObjects;

namespace ProductService.Application.Features.Commands.Product.ConfirmProductForOrder
{
    public class ConfirmProductForOrderCommandHandler : IRequestHandler<ConfirmProductForOrderCommand, ConfirmProductForOrderCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private int nCount;

        public ConfirmProductForOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            nCount = 0;
        }

        public async Task<ConfirmProductForOrderCommandResponse> Handle(ConfirmProductForOrderCommand request, CancellationToken cancellationToken)
        {
            foreach (var productDetail in request.ProductModels.ProductDetailModels)
            {
                nCount += 1;
                var product = await _unitOfWork.GetReadRepository<ProductService.Domain.Aggregate.ProductAggregate.Product, ProductId>().GetAsync(p => p.Id == ProductId.Create(productDetail.ProductId));

                if (product.StockCount >= productDetail.ProductCount.Value)
                    product.StockCount -= productDetail.ProductCount.Value;
                else
                    return new(false);

                if (request.ProductModels.ProductDetailModels.Count() == nCount)
                    return new(await _unitOfWork.SaveChangesAsync() > 0);
            }
            return new(false);
        }
    }
}
