using System.ComponentModel.DataAnnotations;

namespace FoodDeliverySampleApplication.API
{
    public class CartDTO
    {
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int ItemCount { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
