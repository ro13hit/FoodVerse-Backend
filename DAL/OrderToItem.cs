using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliverySampleApplication.DAL
{
    public class OrderToItem
    {
        public int OrderToItemId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int ItemCount { get; set; }
    }
}
