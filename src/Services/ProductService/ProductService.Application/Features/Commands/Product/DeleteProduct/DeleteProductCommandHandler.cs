using BuildingBlock.Base.Abstractions;
using MediatR;
using ProductService.Domain.Aggregate.ProductAggregate.Events;
using ProductService.Domain.Aggregate.ProductAggregate.ValueObjects;
using ProductService.Domain.Constants;

namespace ProductService.Application.Features.Commands.Product.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, DeleteProductCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private UpdateMemoryDataEvent updateMemoryData = null;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            updateMemoryData = new(Constant.TableNames.Products);
        }

        public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = ProductService.Domain.Aggregate.ProductAggregate.Product.Create(request.ProductId);

            product.AddProductDomainEvent(updateMemoryData);

            bool dbResult = await _unitOfWork.GetWriteRepository<ProductService.Domain.Aggregate.ProductAggregate.Product, ProductId>().DeleteByIdAsync(product);

            dbResult = await _unitOfWork.SaveChangesAsync() > 0;

            return new(dbResult);
        }
    }
}
