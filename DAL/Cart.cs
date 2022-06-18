using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliverySampleApplication.DAL
{
    public partial class Cart
    {
        public List<ItemList> Items = new();
        public int UserId { get; set; }
    }
}
