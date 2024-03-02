namespace BasketService.Application.Dtos
{
    public class AddBasketDto
    {
        public string orderDescription { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
    }
}
