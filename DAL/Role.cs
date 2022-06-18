using System;
using System.Collections.Generic;

namespace FoodDeliverySampleApplication.DAL
{
    public partial class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; } = null!;
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
