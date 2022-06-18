using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliverySampleApplication.DAL
{
    public partial class Order
    {
        public Order()
        {
            OrderToItems = new HashSet<OrderToItem>();
        }

        [Key]
        public int OrderId { get; set; }
        public string Address { get; set; } = null!;
        public decimal TotalBill { get; set; }
        public string Comments { get; set; } = string.Empty!;
        public int UserId { get; set; }
        public int? Rating { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<OrderToItem> OrderToItems { get; set; }
    }
}
