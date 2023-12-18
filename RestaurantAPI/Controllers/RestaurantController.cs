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

        public RestaurantController(IRestaurantServices restaurantServices)
        {
            _restaurantServices = restaurantServices;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAllRestaurants()
        {

            var restaurantsDto = _restaurantServices.GetAll();

            return Ok(restaurantsDto);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<RestaurantDto> GetRestaurantById([FromRoute] int id)
        {

            var restaurantDto = _restaurantServices.GetById(id);
            if (restaurantDto is null)
                return NotFound();

            return Ok(restaurantDto);
        }

        [HttpPost]
        public ActionResult<IEnumerable<RestaurantDto>> CreateRestaurants([FromBody] RestaurantCreateDto restaurantCreateDto)
        {
            int id = _restaurantServices.Create(restaurantCreateDto);

            return Created($"api/Restaurant/{id}", null);

        }

        [HttpDelete]
        public ActionResult DeleteRestaurant([FromBody] string name)
        {
            if (!_restaurantServices.Delete(name))
                return NotFound();

            return NoContent();
        }
    }
}
