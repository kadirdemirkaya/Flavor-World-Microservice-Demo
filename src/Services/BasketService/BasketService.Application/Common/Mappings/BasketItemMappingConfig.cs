using AutoMapper;
using BasketService.Domain.Aggregate.Entities;
using BasketService.Domain.Models;

namespace BasketService.Application.Common.Mappings
{
    public class BasketItemMappingConfig : Profile
    {
        public BasketItemMappingConfig()
        {
            CreateMap<BasketItem, BasketItemModel>().ReverseMap();
        }
    }
}
