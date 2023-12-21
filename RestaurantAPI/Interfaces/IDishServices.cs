using RestaurantAPI.Models.Dtos;

namespace RestaurantAPI.Interfaces
{
    public interface IDishServices
    {
        void DeleteById(int restaurantId, int dishId);
        void DeleteDishes(int restaurantId);
        IEnumerable<DishDto> GetAllDishes(int restaurantId);
        DishDto GetDishById(int restaurantId, int dishId);
        int PostDish(int restaurantId, DishCreateDto dishCreateDto);
        void PutDishes(int restaurantId, int dishId, DishUpdateDto dishUpdateDto);
    }
}
