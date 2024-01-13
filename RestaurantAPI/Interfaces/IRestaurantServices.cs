using RestaurantAPI.Models;
using RestaurantAPI.Models.Dtos;
using System.Security.Claims;

namespace RestaurantAPI.Interfaces
{
    public interface IRestaurantServices
    {
        PagedResult<RestaurantDto> GetAll(RestaurantQuery query);
        RestaurantDto GetById(int id);
        int Create(RestaurantCreateDto restaurantDto);
        void Delete(string name);
        void Update(int id, RestaurantUpdateDto updateRestaurantDto);
    }
}
