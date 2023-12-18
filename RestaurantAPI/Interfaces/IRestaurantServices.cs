using RestaurantAPI.Models.Dtos;

namespace RestaurantAPI.Interfaces
{
    public interface IRestaurantServices
    {
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto GetById(int id);
        int Create(RestaurantCreateDto restaurantDto);
        bool Delete(string name);
        bool Update(int id, RestaurantUpdateDto updateRestaurantDto);
    }
}
