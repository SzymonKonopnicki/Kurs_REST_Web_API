using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models;
using RestaurantAPI.Models.Dtos;
using System.Linq.Expressions;
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

        public PagedResult<RestaurantDto> GetAll(RestaurantQuery query)
        {
            var baseQuery = _dbContext.Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dish)
                .Where(x => query.SearchPhrase == null || (x.Name.ToLower().Contains(query.SearchPhrase.ToLower()) ||
                                                x.Description.ToLower().Contains(query.SearchPhrase.ToLower())));
            
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    { nameof(Restaurant.Name), x => x.Name },
                    { nameof(Restaurant.Description), x => x.Description },
                    { nameof(Restaurant.Category), x => x.Category }
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC ? baseQuery.OrderBy(selectedColumn) : baseQuery.OrderByDescending(selectedColumn);
            }
            
            var restaurants = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            var result = new PagedResult<RestaurantDto>(restaurantsDto, baseQuery.Count(), query.PageSize, query.PageNumber);

            return result;
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
