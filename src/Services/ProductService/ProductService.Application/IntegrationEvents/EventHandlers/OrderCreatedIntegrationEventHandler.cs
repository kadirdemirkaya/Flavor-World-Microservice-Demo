using BuildingBlock.Base.Abstractions;
using BuildingBlock.Redis;
using MediatR;
using ProductService.Application.Features.Commands.Product.ConfirmProductForOrder;
using ProductService.Application.IntegrationEvents.Events;
using ProductService.Domain.Constants;
using ProductService.Domain.Models;
using BuildingBlock.Base.Enums;

namespace ProductService.Application.IntegrationEvents.EventHandlers
{
    public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        private readonly IRedisRepository<ProductService.Domain.Models.ProductModel> _redisProductRepository;
        private readonly IMediator _mediator;
        private readonly IEventBus _eventBus;

        public OrderCreatedIntegrationEventHandler(IRedisRepository<Domain.Models.ProductModel> redisProductRepository, IMediator mediator, IEventBus eventBus)
        {
            _redisProductRepository = redisProductRepository;
            _mediator = mediator;
            _eventBus = eventBus;
        }

        public async Task Handle(OrderCreatedIntegrationEvent @event)
        {
            ProductService.Domain.Models.ProductModel productModel = new ProductModel(@event.OrderDescription, @event.ProductDetailModels);

            bool redisResult = await _redisProductRepository.CreateAsync(Constant.InMemory.InMemoryProductKey, productModel, BuildingBlock.Base.Enums.RedisDataType.String);

            if (redisResult)
            {
                ProductModel? productModels = await _redisProductRepository.GetByIdAsync(Constant.InMemory.InMemoryProductKey, Constant.InMemory.InMemoryProductKeyId.ToString(),
                    RedisDataType.String);

                ConfirmProductForOrderCommand confirmProductForOrder = new(productModels);
                ConfirmProductForOrderCommandResponse response = await _mediator.Send(confirmProductForOrder);

                if (response.result is true)
                {
                    _redisProductRepository.Delete(Constant.InMemory.InMemoryProductKey, Constant.InMemory.InMemoryProductKeyId.ToString(), RedisDataType.String);
                    OrderCompletedIntegrationEvent orderCompleted = new(@event.BasketId, @event.OrderId, @event.UserId, @event.Token);
                    _eventBus.Publish(orderCompleted);
                }
                else
                {
                    OrderNotCompletedIntegrationEvent orderNotCompleted = new(@event.BasketId, @event.OrderId, @event.UserId, @event.Token);
                    _eventBus.Publish(orderNotCompleted);
                }
            }
        }
    }
}
