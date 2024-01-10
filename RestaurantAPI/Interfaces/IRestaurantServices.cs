using RestaurantAPI.Models.Dtos;
using System.Security.Claims;

namespace RestaurantAPI.Interfaces
{
    public interface IRestaurantServices
    {
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto GetById(int id);
        int Create(RestaurantCreateDto restaurantDto);
        void Delete(string name);
        void Update(int id, RestaurantUpdateDto updateRestaurantDto);
    }
}
