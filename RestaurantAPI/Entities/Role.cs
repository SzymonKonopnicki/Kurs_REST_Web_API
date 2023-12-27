using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Entities
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
