namespace BasketService.Application.Dtos
{
    public class BasketItemDto
    {
        public Guid BasketId { get; set; }
        public Guid ProductId { get; set; }

        public int? ProductCount { get; set; } = 1;
    }
}
