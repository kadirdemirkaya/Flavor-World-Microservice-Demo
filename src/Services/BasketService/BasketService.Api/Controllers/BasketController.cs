using BasketService.Application.Dtos;
using BasketService.Application.Features.Commands.Basket.AddBasketInCache;
using BasketService.Application.Features.Commands.Basket.ConfirmBasketForOrder;
using BasketService.Application.Features.Commands.Basket.RemoveBasketItemInCache;
using BasketService.Application.Features.Queries.Basket.GetAllBasketInCache;
using BasketService.Application.Features.Queries.Basket.GetBasketItem;
using BuildingBlock.Base.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediatr;
        private readonly ITokenService _tokenService;

        public BasketController(IMediator mediatr, ITokenService tokenService)
        {
            _mediatr = mediatr;
            _tokenService = tokenService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Get-All-Basket-In-Cache")]
        public async Task<IActionResult> GetAllBasketInCache([FromHeader] Guid basketId)
        {
            GetAllBasketInCacheQuery getAllBasketInCache = new(basketId);
            GetAllBasketInCacheQueryResponse? response = await _mediatr.Send(getAllBasketInCache);
            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Add-Basket-In-Cache")]
        public async Task<IActionResult> AddBasketInCache([FromBody] AddBasketDto addBasketDto)
        {
            AddBasketInCacheCommand addBasketInCacheCommand = new(addBasketDto);
            AddBasketInCacheCommandResponse response = await _mediatr.Send(addBasketInCacheCommand);
            return response.result ? Ok(response.result) : BadRequest(response.result);
        }

        [HttpDelete]
        [AllowAnonymous]
        [Route("Remove-Basket-Item-In-Cache")]
        public async Task<IActionResult> RemoveBasketItemInCache([FromHeader] Guid productId)
        {
            RemoveBasketItemInCacheCommand removeBasketItemInCache = new(productId);
            RemoveBasketItemInCacheCommandResponse response = await _mediatr.Send(removeBasketItemInCache);
            return response.Result ? Ok(response.Result) : BadRequest(response.Result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Get-Basket-Item")]
        public async Task<IActionResult> GetBasketItem([FromHeader] Guid productId)
        {
            GetBasketItemQuery getBasketItem = new(productId);
            GetBasketItemQueryResponse response = await _mediatr.Send(getBasketItem);
            return Ok(response.BasketItemDto);
        }

        [HttpPost]
        [Route("Confirm-Basket-For-Order")]
        public async Task<IActionResult> ConfirmBasketForOrder()
        {
            ConfirmBasketForOrderCommand confirmBasketForOrderCommand = new(_tokenService.GetTokenInHeader());
            ConfirmBasketForOrderCommandResponse response = await _mediatr.Send(confirmBasketForOrderCommand);
            return response.result ? Ok(response.result) : BadRequest(response.result);
        }
    }
}
