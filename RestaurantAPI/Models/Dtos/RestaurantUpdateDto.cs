using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models.Dtos
{
    public class RestaurantUpdateDto
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }
        [Required]
        public bool HasDelivery { get; set; }

    }
}
