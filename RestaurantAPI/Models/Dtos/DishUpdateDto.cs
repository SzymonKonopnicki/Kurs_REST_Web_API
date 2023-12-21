using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models.Dtos
{
    public class DishUpdateDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

    }
}
