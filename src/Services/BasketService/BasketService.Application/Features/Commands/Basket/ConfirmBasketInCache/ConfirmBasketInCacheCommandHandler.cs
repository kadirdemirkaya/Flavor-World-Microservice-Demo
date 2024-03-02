using BasketService.Application.Abstractions;
using BasketService.Domain.Aggregate.ValueObjects;
using BasketService.Domain.Constants;
using BasketService.Domain.Models;
using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Enums;
using BuildingBlock.Redis;
using MediatR;

namespace BasketService.Application.Features.Commands.Basket.ConfirmBasketInCache
{
    public class ConfirmBasketInCacheCommandHandler : IRequestHandler<ConfirmBasketInCacheCommand, ConfirmBasketInCacheCommandResponse>
    {
        private readonly IRedisRepository<BasketItemModel> _redisRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfoService _userInfoService;

        public ConfirmBasketInCacheCommandHandler(IRedisRepository<BasketItemModel> redisRepository, IUnitOfWork unitOfWork, IUserInfoService userInfoService)
        {
            _redisRepository = redisRepository;
            _unitOfWork = unitOfWork;
            _userInfoService = userInfoService;
        }

        public async Task<ConfirmBasketInCacheCommandResponse> Handle(ConfirmBasketInCacheCommand request, CancellationToken cancellationToken)
        {
            List<BasketItemModel>? redisItemModels = request.BasketItemModels;

            var userModel = await _userInfoService.GetUserModel(request.token);

            if (redisItemModels is not null && redisItemModels.Count() > 0)
            {
                BasketService.Domain.Aggregate.Basket? basket = BasketService.Domain.Aggregate.Basket.Create(BasketId.Create(redisItemModels.FirstOrDefault().BasketId), DateTime.Now.ToString(), Domain.Aggregate.Enums.BasketStatus.InActive, UserId.Create(userModel.Id));

                foreach (var basketItemModel in redisItemModels)
                    basket.AddBasketItem(BasketId.Create(basketItemModel.BasketId), ProductId.Create(basketItemModel.ProductId), DateTime.Now.ToString(), true);

                bool dbResult = await _unitOfWork.GetWriteRepository<BasketService.Domain.Aggregate.Basket, BasketId>().CreateAsync(basket);

                if (dbResult)
                    return new(await _unitOfWork.SaveChangesAsync() > 0);
            }
            return new(false);
        }
    }
}
