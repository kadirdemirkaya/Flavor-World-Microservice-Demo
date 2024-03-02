using FluentValidation;
using ProductService.Application.Dtos;

namespace ProductService.Application.Features.Commands.Product.CreateProduct
{
    //public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    //{
    //    public CreateProductCommandValidator()
    //    {
    //        RuleFor(p => p.CreatedDate).NotNull().NotEmpty().WithMessage("Created Date not empty");

    //        RuleFor(p => p.Description).NotNull().NotEmpty().WithMessage("Description not empty")
    //            .MaximumLength(3).WithMessage("Description must be max 5 charavter");

    //        RuleFor(p => p.ProductCategory).NotNull().NotEmpty().WithMessage("ProductCategory not empty");

    //        RuleFor(p => p.Price).NotNull().NotEmpty().WithMessage("Price not empty");

    //        RuleFor(p => p.StockQuantity).NotNull().NotEmpty().WithMessage("StockQuantity not empty");

    //        RuleFor(p => p.ProductName).NotNull().NotEmpty().WithMessage("ProductName not empty")
    //            .MaximumLength(3).WithMessage("ProductName must be max 5 charavter"); ;

    //        RuleFor(p => p.ProductStatus).NotNull().NotEmpty().WithMessage("ProductStatus not empty");
    //    }
    //}

    //public class CreateProductCommandDtoValidator : AbstractValidator<CreateProductCommandDto>
    //{
    //    public CreateProductCommandDtoValidator()
    //    {
    //        RuleFor(p => p.CreatedDate).NotNull().NotEmpty().WithMessage("Created Date not empty");

    //        RuleFor(p => p.Description).NotNull().NotEmpty().WithMessage("Description not empty")
    //            .MaximumLength(3).WithMessage("Description must be max 5 charavter");

    //        RuleFor(p => p.ProductCategory).NotNull().NotEmpty().WithMessage("ProductCategory not empty");

    //        RuleFor(p => p.Price).NotNull().NotEmpty().WithMessage("Price not empty");

    //        RuleFor(p => p.StockQuantity).NotNull().NotEmpty().WithMessage("StockQuantity not empty");

    //        RuleFor(p => p.ProductName).NotNull().NotEmpty().WithMessage("ProductName not empty")
    //            .MaximumLength(3).WithMessage("ProductName must be max 5 charavter"); ;

    //        RuleFor(p => p.ProductStatus).NotNull().NotEmpty().WithMessage("ProductStatus not empty");
    //    }
    //}
}
