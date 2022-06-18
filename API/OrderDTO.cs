using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliverySampleApplication.API
{
    public class OrderDTO
    {
        public string Address { get; set; } = null!;
        public decimal TotalBill { get; set; }
        public string Comments { get; set; } = string.Empty!;
        [Required]
        public int UserId { get; set; }
        public int? Rating { get; set; }
    }
}
