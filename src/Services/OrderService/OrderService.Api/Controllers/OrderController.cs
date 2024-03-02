using BuildingBlock.Base.Abstractions;
using BuildingBlock.InMemory;
using Microsoft.AspNetCore.Mvc;
using OrderService.Domain.Aggregate.OrderAggregate;
using OrderService.Domain.Aggregate.OrderAggregate.ValueObjects;

namespace OrderService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IWriteRepository<Order, OrderId> _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IInMemoryRepository<Order, OrderId> _inMemoryRepository;

        public OrderController(IInMemoryRepository<Order, OrderId> inMemoryRepository, IUnitOfWork unitOfWork)
        {
            _inMemoryRepository = inMemoryRepository;
            _unitOfWork = unitOfWork;
        }

        public OrderController(IWriteRepository<Order, OrderId> orderRepository, IUnitOfWork unitOfWork, IInMemoryRepository<Order, OrderId> inMemoryRepository)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _inMemoryRepository = inMemoryRepository;
        }



        //[HttpPost]
        //[Route("Create-Order")]
        //public async Task<IActionResult> CreateOrder(string description)
        //{
        //    var result = await _orderRepository.CreateAsync(Order.Create(description, DateTime.Now));
        //    await _unitOfWork.SaveChangesAsync();
        //    return Ok(result);
        //}

        //[HttpPost]
        //[Route("Create-Order-InMemory")]
        //public async Task<IActionResult> CreateOrderInMemory(string description)
        //{
        //    var result = await _inMemoryRepository.CreateAsync(Order.Create(description, DateTime.Now));
        //    await _unitOfWork.SaveChangesAsync();
        //    var dataResult = await _inMemoryRepository.GetAllAsync();
        //    return Ok(dataResult);
        //}
    }
}
