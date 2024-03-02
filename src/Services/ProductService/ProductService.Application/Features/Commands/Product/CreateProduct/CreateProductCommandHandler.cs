using BuildingBlock.Base.Abstractions;
using MediatR;
using ProductService.Application.Abstractions;
using ProductService.Domain.Aggregate.ProductAggregate.Events;
using ProductService.Domain.Aggregate.ProductAggregate.ValueObjects;
using ProductService.Domain.Constants;

namespace ProductService.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionService<string> _sessionService;
        private readonly IProductImageService _productImageService;
        private UpdateMemoryDataEvent updateMemoryData = null;
        public CreateProductCommandHandler(IUnitOfWork unitOfWork, ISessionService<string> sessionService, IProductImageService productImageService)
        {
            _unitOfWork = unitOfWork;
            _sessionService = sessionService;
            _productImageService = productImageService;
            updateMemoryData = new(Constant.TableNames.Products);
        }

        public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            ProductService.Domain.Aggregate.ProductAggregate.Product? product = ProductService.Domain.Aggregate.ProductAggregate.Product.Create(request.ProductName, request.Description, request.CreatedDate, request.ProductStatus, request.StockQuantity, request.Price, request.ProductCategory, ProductDetail.Create(request.Price, request.StockQuantity));

            bool result = await _unitOfWork.GetWriteRepository<ProductService.Domain.Aggregate.ProductAggregate.Product, ProductId>().
                CreateAsync(product);

            bool imageResult = await _productImageService.ProductImageAddAsync(new() { ProductId = product.Id.Id }, new() { File = null, Name = "DefaultProductImage", Path = "/Image/DefaultProductImage" });

            if (imageResult)
            {
                product.AddProductDomainEvent(new SendEmailEvent(_sessionService.GetSessionValue("email")));

                product.AddProductDomainEvent(updateMemoryData);

                int dbResult = await _unitOfWork.SaveChangesAsync();

                return dbResult <= 0 ? false : true;
            }
            return false;
        }
    }
}
