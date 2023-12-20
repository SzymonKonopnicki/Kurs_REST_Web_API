using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models.Dtos;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private IRestaurantServices _restaurantServices;
        private ILogger _logger;
        public RestaurantController(IRestaurantServices restaurantServices, ILogger<RestaurantController> logger)
        {
            _restaurantServices = restaurantServices;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAllRestaurants()
        {
            var restaurantsDto = _restaurantServices.GetAll();

            _logger.LogInformation("Sending list of restaurations");

            return Ok(restaurantsDto);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<RestaurantDto> GetRestaurantById([FromRoute] int id)
        {
            var restaurantDto = _restaurantServices.GetById(id);

            _logger.LogInformation($"Sending restauration with Id:{id}");

            return Ok(restaurantDto);
        }

        [HttpPost]
        public ActionResult<IEnumerable<RestaurantDto>> CreateRestaurants([FromBody] RestaurantCreateDto restaurantCreateDto)
        {
            int id = _restaurantServices.Create(restaurantCreateDto);

            _logger.LogInformation($"Create restauration with Id:{id}");
            return Created($"api/Restaurant/{id}", null);
        }

        [HttpDelete]
        public ActionResult DeleteRestaurant([FromBody] string name)
        {
            _logger.LogWarning($"Delete restauration with Name:{name}");
            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult UpdateRestaurant([FromRoute] int id, [FromBody] RestaurantUpdateDto updateRestaurantDto)
        {
            _restaurantServices.Update(id, updateRestaurantDto);

            _logger.LogWarning($"Update restauration with Id:{id}");
            return Ok();
        }
    }
}
