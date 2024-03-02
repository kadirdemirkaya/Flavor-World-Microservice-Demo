using AutoMapper;
using ProductService.Application.Dtos;
using ProductService.Application.Features.Commands.Product.CreateProduct;
using ProductService.Domain.Aggregate.ProductAggregate;
using ProductService.Domain.Models;

namespace ProductService.Application.Common.Mappings
{
    public class ProductMappingConfig : Profile
    {
        public ProductMappingConfig()
        {
            CreateMap<CreateProductCommandDto, CreateProductCommand>().ReverseMap();
            CreateMap<AllProductModel, Product>().ReverseMap();
            CreateMap<AllProductsModel, Product>().ReverseMap();
            CreateMap<AllProductsModel, AllProductModel>().ReverseMap();
            CreateMap<UpdateProductModel, Product>().ReverseMap();
            CreateMap<ProductElasticModel, Product>().ReverseMap();
        }
    }
}
