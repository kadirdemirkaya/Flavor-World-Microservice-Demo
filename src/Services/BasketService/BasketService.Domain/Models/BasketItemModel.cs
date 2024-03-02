using BuildingBlock.Base.Models.Base;

namespace BasketService.Domain.Models
{
    public class BasketItemModel : RedisModel
    {
        public Guid Id { get; set; }
        public Guid BasketId { get; set; }
        public Guid ProductId { get; set; }

        public string? OrderDescription { get; set; } = "NONE";
        public int? ProductCount { get; set; } = 1;
    }
}
