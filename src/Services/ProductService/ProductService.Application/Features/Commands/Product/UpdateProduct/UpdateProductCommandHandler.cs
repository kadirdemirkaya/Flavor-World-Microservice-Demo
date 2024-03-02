using AutoMapper;
using BuildingBlock.Base.Abstractions;
using MediatR;
using ProductService.Domain.Aggregate.ProductAggregate.Events;
using ProductService.Domain.Aggregate.ProductAggregate.ValueObjects;
using ProductService.Domain.Constants;

namespace ProductService.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private UpdateMemoryDataEvent updateMemoryData = null;
        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            updateMemoryData = new(Constant.TableNames.Products);
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<ProductService.Domain.Aggregate.ProductAggregate.Product>(request.UpdateProductModel);
            product.AddProductDomainEvent(updateMemoryData);
            bool dbResult = _unitOfWork.GetWriteRepository<ProductService.Domain.Aggregate.ProductAggregate.Product, ProductId>().UpdateAsync(product);
            if (dbResult)
            {
                await _unitOfWork.SaveChangesAsync();
                return new(request.UpdateProductModel);
            }
            return new(default);
        }
    }
}
