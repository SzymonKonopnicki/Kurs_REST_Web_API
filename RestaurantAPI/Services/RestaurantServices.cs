using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models.Dtos;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public class RestaurantServices : IRestaurantServices
    {
        private RestaurantDbContext _dbContext;
        private IMapper _mapper;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextServic _userContextServic;

        public RestaurantServices(RestaurantDbContext dbContext, IMapper mapper, IAuthorizationService authorization, IUserContextServic userContextServic)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _authorization = authorization;
            _userContextServic = userContextServic;
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
            restaurant.CreatedById = _userContextServic.GetUserId;
            _dbContext.AddRange(restaurant);
            _dbContext.SaveChanges();

            return (restaurant.Id);
        }

        public void Delete(string name)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(x => x.Name == name);

            if (restaurant is null)
                throw new NotFoundException("Not found");

            var authorizationResult = _authorization.AuthorizeAsync(_userContextServic.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
                throw new ForbidException();


            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
        }

        public void Update(int id, RestaurantUpdateDto updateRestaurantDto)
        {

            var restaurant = _dbContext.Restaurants.FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Not found");

            var authorizationResult = _authorization.AuthorizeAsync(_userContextServic.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
                throw new ForbidException();

            restaurant.Name = updateRestaurantDto.Name;
            restaurant.Description = updateRestaurantDto.Description;
            restaurant.HasDelivery = updateRestaurantDto.HasDelivery;

            _dbContext.SaveChanges();
        }
    }

}
