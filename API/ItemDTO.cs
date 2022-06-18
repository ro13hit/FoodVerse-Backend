using System.ComponentModel.DataAnnotations;

namespace FoodDeliverySampleApplication.API
{
    public class ItemDTO
    {
        [Required, MaxLength(30)]
        public string Name { get; set; }
        [Required, MaxLength(30)]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Quantity { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public int IsAvailable { get; set; }
        public byte[] Image { get; set; }
    }
}
