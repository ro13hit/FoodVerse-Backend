using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliverySampleApplication.DAL
{
    public partial class Item
    {
        [Key]
        public int ItemId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Quantity { get; set; } = string.Empty!;
        public int CategoryId { get; set; }
        public int IsAvailable { get; set; }
        public int IsDeleted { get; set; }
        public byte[] Image { get; set; }
        public virtual Category Category { get; set; }
    }
}
