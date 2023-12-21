using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models.Dtos;

namespace RestaurantAPI.Services
{
    public class DishServices : IDishServices
    {
        private RestaurantDbContext _dbContext;
        private IMapper _mapper;

        public DishServices(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<DishDto> GetAllDishes(int restaurantId)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(x => x.Dish)
                .FirstOrDefault(x => x.Id == restaurantId);

            if (restaurant == null)
                throw new NotFoundException("Not found restaurant.");

            var dishesDto = _mapper.Map<List<DishDto>>(restaurant.Dish.ToList());

            return dishesDto;
        }

        public DishDto GetDishById(int restaurantId, int dishId)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(x => x.Dish)
                .FirstOrDefault(x => x.Id == restaurantId);

            if (restaurant == null)
                throw new NotFoundException("Not found restaurant.");

            var dishDto = _mapper.Map<DishDto>(restaurant.Dish.FirstOrDefault(x => x.Id == dishId));

            return dishDto;
        }

        public int PostDish(int restaurantId, DishCreateDto dishCreateDto)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(x => x.Dish)
                .FirstOrDefault(x => x.Id == restaurantId);

            if (restaurant == null)
                throw new NotFoundException("Not found restaurant.");

            var dish = _mapper.Map<Dish>(dishCreateDto);
            
            dish.RestaurantId = restaurantId;

            _dbContext.AddRange(dish);
            _dbContext.SaveChanges();
            
            return dish.Id;
        }

        public void DeleteById(int restaurantId, int dishId)
        {
            var dish = _dbContext
                .Dishes
                .FirstOrDefault(x => x.Id == dishId);

            if (dish == null)
                throw new NotFoundException("Not found dish.");

            _dbContext.Dishes.Remove(dish);
            _dbContext.SaveChanges();
        }

        public void DeleteDishes(int restaurantId)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(x => x.Dish)
                .FirstOrDefault(x => x.Id == restaurantId);

            if (restaurant == null)
                throw new NotFoundException("Not found restaurant.");

            _dbContext.RemoveRange(restaurant.Dish);
            _dbContext.SaveChanges();
        }

        public void PutDishes(int restaurantId, int dishId, DishUpdateDto dishUpdateDto)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(x => x.Dish)
                .FirstOrDefault(x => x.Id == restaurantId);

            if (restaurant == null)
                throw new NotFoundException("Not found restaurant.");

            var dish = restaurant.Dish.FirstOrDefault(x => x.Id == dishId);

            if (dish == null)
                throw new NotFoundException("Not found dish.");

            dish.Name = dishUpdateDto.Name;
            dish.Description = dishUpdateDto.Description;
            dish.Price = dishUpdateDto.Price;

            _dbContext.SaveChanges();
        }
    }
}
