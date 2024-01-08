using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models.Dtos;
using RestaurantAPI.Services;
using System.Security.Claims;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [AllowAnonymous]
        public ActionResult<IEnumerable<RestaurantDto>> GetAllRestaurants()
        {
            var restaurantsDto = _restaurantServices.GetAll();

            _logger.LogInformation("Sending list of restaurations");

            return Ok(restaurantsDto);
        }

        [HttpGet]
        [AllowAnonymous]
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
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            int id = _restaurantServices.Create(restaurantCreateDto, userId);

            _logger.LogInformation($"Create restauration with Id:{id}");
            return Created($"api/Restaurant/{id}", null);
        }

        [HttpDelete]
        public ActionResult DeleteRestaurant([FromBody] string name)
        {
            _restaurantServices.Delete(name, User);
            _logger.LogWarning($"Delete restauration with Name:{name}");
            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult UpdateRestaurant([FromRoute] int id, [FromBody] RestaurantUpdateDto updateRestaurantDto)
        {
            _restaurantServices.Update(id, updateRestaurantDto, User);

            _logger.LogWarning($"Update restauration with Id:{id}");
            return Ok();
        }
    }
}
