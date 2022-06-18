using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliverySampleApplication.API
{
    public class ItemResponse
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Quantity { get; set; }
        public int CategoryId { get; set; }
        public int IsAvailable { get; set; }
        public byte[] Image { get; set; }
    }
}
