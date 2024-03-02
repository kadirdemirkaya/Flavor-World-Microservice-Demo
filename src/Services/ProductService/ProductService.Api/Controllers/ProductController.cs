using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Dtos;
using ProductService.Application.Features.Commands.Product.CreateProduct;
using ProductService.Application.Features.Commands.Product.DeleteProduct;
using ProductService.Application.Features.Commands.Product.UpdateProduct;
using ProductService.Application.Features.Queries.Product.FilterProductCategory;
using ProductService.Application.Features.Queries.Product.GetAllProduct;
using ProductService.Application.Features.Queries.Product.GetProduct;
using ProductService.Application.Features.Queries.Product.SearchProductName;
using ProductService.Domain.Aggregate.ProductAggregate.Enums;
using ProductService.Domain.Models;
using ProductService.Infrastructure.Attributes;
using System.Net;

namespace ProductService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Guest")]
    [UserRequestAttributeActionFilter]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediatr;
        private readonly IMapper _mapper;

        public ProductController(IMediator mediatr, IMapper mapper)
        {
            _mediatr = mediatr;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Create-Product")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommandDto createProductCommandDto)
        {
            bool result = await _mediatr.Send(_mapper.Map<CreateProductCommand>(createProductCommandDto));
            return result is true ? Ok(true) : BadRequest(false);
        }

        [HttpDelete]
        [Route("Delete-Product")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteProduct([FromHeader] Guid productId)
        {
            DeleteProductCommandRequest request = new(productId);
            DeleteProductCommandResponse response = await _mediatr.Send(request);
            return response.Result is true ? Ok(true) : BadRequest(false);
        }

        [HttpGet]
        [Route("Get-All-Product")]
        [ProducesResponseType(typeof(List<AllProductModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetAllProduct()
        {
            GetAllProductQueryRequest request = new();
            GetAllProductQueryResponse response = await _mediatr.Send(request);
            if (response.AllProductModel is null || response.AllProductModel.Count() == 0)
                return NoContent();
            return Ok(response.AllProductModel);
        }

        [HttpGet]
        [Route("Get-Product")]
        [ProducesResponseType(typeof(List<AllProductModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetProduct([FromHeader] Guid productId)
        {
            GetProductQueryRequest request = new(productId);
            GetProductQueryResponse response = await _mediatr.Send(request);
            if (response.AllProductModel is null)
                return NoContent();
            return Ok(response.AllProductModel);
        }

        [HttpPut]
        [Route("Update-Product")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductModel updateProductModel) // ! <-Dto-> !
        {
            UpdateProductCommandRequest request = new(updateProductModel);
            UpdateProductCommandResponse response = await _mediatr.Send(request);
            if (response.UpdateProduct is null)
                return NoContent();
            return Ok(response.UpdateProduct);
        }

        [HttpGet]
        [Route("Search-Product-Name")]
        [ProducesResponseType(typeof(List<AllProductModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> SearchProductName([FromQuery] string word)
        {
            SearchProductNameQueryRequest request = new(word);
            SearchProductNameQueryResponse response = await _mediatr.Send(request);
            if (response.AllProductModels is null)
                return NoContent();
            return Ok(response.AllProductModels);
        }

        [HttpGet]
        [Route("Filter-Product-Category")]
        [ProducesResponseType(typeof(List<ProductElasticModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> FilterProductCategory([FromQuery] ProductCategory productCategory)
        {
            FilterProductCategoryQueryRequest request = new(productCategory);
            FilterProductCategoryQueryResponse response = await _mediatr.Send(request);
            if (response.ProductElasticModels is null || response.ProductElasticModels.Count() == 0)
                return NoContent();
            return Ok(response.ProductElasticModels);
        }
    }
}
