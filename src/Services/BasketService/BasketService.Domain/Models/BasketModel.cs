using BasketService.Domain.Aggregate.Enums;
using BuildingBlock.Base.Models.Base;

namespace BasketService.Domain.Models
{
    public class BasketModel : RedisModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public BasketStatus BasketStatus { get; set; } = BasketStatus.InActive;
    }
}
