using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models.Dtos;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private RestaurantDbContext _dbContext;
        private IMapper _mapper;

        public RestaurantController(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAllRestaurants()
        {
            var restaurants = _dbContext.Restaurants
                .Include(x => x.Dish)
                .Include(x => x.Address)
                .ToList();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            return Ok(restaurantsDto);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<RestaurantDto> GetRestaurantById([FromRoute] int id)
        {
            var restaurant = _dbContext.Restaurants
                .Include(x => x.Dish)
                .Include(x => x.Address)
                .FirstOrDefault(x => x.Id == id);

            if (restaurant == null)
            {
                return NotFound();
            }

            var resyaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            return Ok(resyaurantDto);
        }

        [HttpPost]
        public ActionResult<IEnumerable<RestaurantDto>> CreateRestaurants([FromBody] RestaurantCreateDto restaurantCreateDto)
        {
            var restaurant = _mapper.Map<Restaurant>(restaurantCreateDto);

            _dbContext.AddRange(restaurant);
            _dbContext.SaveChanges();

            return Created($"api/Restaurant/{restaurant.Id}", null);

        }
    }
}
