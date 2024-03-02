using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models;
using ImageService.Application.Features.Commands.Image.ImageAdd;
using ImageService.Application.Features.Commands.Image.ProductImageAdd;
using ImageService.Application.Features.Commands.Image.UserImageAdd;
using ImageService.Application.Features.Queries.Image;
using ImageService.Application.Features.Queries.Image.UserImageGet;
using ImageService.Infrastructure.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UserRequestAttributeActionFilter]
    public class ImageController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IMediator _mediatr;

        public ImageController(ITokenService tokenService, IMediator mediatr)
        {
            _tokenService = tokenService;
            _mediatr = mediatr;
        }

        [HttpPost]
        [Route("Image-Add")]
        public async Task<IActionResult> ImageAdd([FromForm] FileUpload fileUpload)
        {
            ImageAddCommand imageAddCommand = new(fileUpload);
            var response = await _mediatr.Send(imageAddCommand);
            return response.result is true ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        [Authorize]
        [Route("User-Image-Add")]
        public async Task<IActionResult> UserImageAdd([FromForm] FileUpload fileUpload)
        {
            UserImageAddCommand imageAddCommand = new(fileUpload, _tokenService.GetTokenInHeader());
            var response = await _mediatr.Send(imageAddCommand);
            return response.result is true ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        [Authorize]
        [Route("User-Image-Get")]
        public async Task<IActionResult> UserImageGet()
        {
            UserImageGetQuery imageGetQuery = new(_tokenService.GetTokenInHeader());
            var response = await _mediatr.Send(imageGetQuery);
            return File(response.Image.Photo, $"{response.Image.FileType}/{response.Image.ContentType}");
        }


        [HttpPost]
        [Route("Product-Image-Add")]
        public async Task<IActionResult> ProductImageAdd([FromForm] FileUpload fileUpload, [FromHeader] Guid productId)
        {
            ProductImageAddCommand productImageAdd = new(new() { ProductId = productId }, fileUpload);
            ProductImageAddCommandResponse? response = await _mediatr.Send(productImageAdd);
            return response.result is true ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        [Route("Product-Image-Get")]
        public async Task<IActionResult> ProductImageGet([FromHeader] Guid productId)
        {
            ProductImageGetQuery userImageGet = new(productId);
            ProductImageGetQueryResponse? response = await _mediatr.Send(userImageGet);
            return File(response.Image.Photo, $"{response.Image.FileType}/{response.Image.ContentType}");
        }
    }
}