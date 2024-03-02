using AuthenticationService.Application.Dtos;
using AuthenticationService.Domain.Aggregate;
using AutoMapper;

namespace AuthenticationService.Application.Common.Mappings
{
    public class UserMappingConfig : Profile
    {
        public UserMappingConfig()
        {
            CreateMap<User, CreateUserDto>().ReverseMap();
        }
    }
}
