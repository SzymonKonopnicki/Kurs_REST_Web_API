using AutoMapper;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Dtos
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(x => x.City, y => y.MapFrom(z => z.Address.City))
                .ForMember(x => x.Street, y => y.MapFrom(z => z.Address.Street))
                .ForMember(x => x.PostalCode, y => y.MapFrom(z => z.Address.PostalCode))
                .ReverseMap();

            CreateMap<Dish, DishDto>();
        }
    }
}
