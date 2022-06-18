
using System.ComponentModel.DataAnnotations;

namespace FoodDeliverySampleApplication.API
{
    public class CategoryDTO
    {
        [Required, MaxLength(30)]
        public string Name { get; set; }
        [Required, MaxLength(30)]
        public string Description { get; set; }
    }
}
