using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models.Dtos;

namespace RestaurantAPI.Services
{
    public class RestaurantServices : IRestaurantServices
    {
        private RestaurantDbContext _dbContext;
        private IMapper _mapper;

        public RestaurantServices(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            var restaurants = _dbContext.Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dish)
                .ToList();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            return restaurantsDto;
        }
        public RestaurantDto GetById(int id)
        {
            var restaurant = _dbContext.Restaurants
                .Include(x => x.Dish)
                .Include(x => x.Address)
                .FirstOrDefault(x => x.Id == id);

            if (restaurant is null) 
                throw new NotFoundException("Not found");

            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            return restaurantDto;
        }

        public int Create(RestaurantCreateDto restaurantCreateDto)
        {
            var restaurant = _mapper.Map<Restaurant>(restaurantCreateDto);
            _dbContext.AddRange(restaurant);
            _dbContext.SaveChanges();

            return (restaurant.Id);
        }

        public bool Delete(string name)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(x => x.Name == name);

            if (restaurant is null)
                throw new NotFoundException("Not found");

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
            return true;
        }

        public bool Update(int id, RestaurantUpdateDto updateRestaurantDto)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Not found");

            restaurant.Name = updateRestaurantDto.Name;
            restaurant.Description = updateRestaurantDto.Description;
            restaurant.HasDelivery = updateRestaurantDto.HasDelivery;

            _dbContext.SaveChanges();
            return true;
        }
    }

}
