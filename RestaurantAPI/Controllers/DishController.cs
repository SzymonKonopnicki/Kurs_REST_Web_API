using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models.Dtos;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private IDishServices _dishServices;

        public DishController(IDishServices dishServices)
        {
            _dishServices = dishServices;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DishDto>> Dishes([FromRoute] int restaurantId)
        {

            IEnumerable<DishDto> result = _dishServices.GetAllDishes(restaurantId);

            return Ok(result);
        }

        [HttpGet]
        [Route("{dishId}")]
        public ActionResult<DishDto> Dish([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            DishDto result = _dishServices.GetDishById(restaurantId, dishId);

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<DishDto> CreateDish([FromRoute] int restaurantId, [FromBody] DishCreateDto dishCreateDto)
        {
            int id = _dishServices.PostDish(restaurantId, dishCreateDto);

            return Created($"api/Restaurant/{restaurantId}/dish/{id}", null);
        }

        [HttpDelete]
        [Route("{dishId}")]
        public ActionResult DeleteDish([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            _dishServices.DeleteById(restaurantId, dishId);
            return NoContent();
        }

        [HttpDelete]
        public ActionResult DeleteDishes([FromRoute] int restaurantId)
        {
            _dishServices.DeleteDishes(restaurantId);
            return NoContent();
        }

        [HttpPut]
        [Route("{dishId}")]
        public ActionResult PutDish([FromRoute] int restaurantId, [FromRoute] int dishId , [FromBody] DishUpdateDto dishUpdateDto)
        {
            _dishServices.PutDishes(restaurantId, dishId, dishUpdateDto);
            return Ok();
        }
    }
}
