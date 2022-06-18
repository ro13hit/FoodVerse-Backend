using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliverySampleApplication.DAL
{
    public partial class Category
    {
        public Category()
        {
            Items = new HashSet<Item>();
        }

        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int IsDeleted { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
