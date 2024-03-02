using BuildingBlock.Base.Models.Base;

namespace OrderService.Domain.Models
{
    public class OrderModel : RedisModel
    {
        public Guid OrderId { get; set; }
        public Guid BasketId { get; set; }
        public string Description { get; set; }
    }
}
