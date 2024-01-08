using RestaurantAPI.Models.Dtos;
using System.Security.Claims;

namespace RestaurantAPI.Interfaces
{
    public interface IRestaurantServices
    {
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto GetById(int id);
        int Create(RestaurantCreateDto restaurantDto, int userId);
        void Delete(string name, ClaimsPrincipal user);
        void Update(int id, RestaurantUpdateDto updateRestaurantDto, ClaimsPrincipal user);
    }
}
