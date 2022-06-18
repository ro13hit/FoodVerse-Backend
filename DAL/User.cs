using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliverySampleApplication.DAL
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
            Roles = new HashSet<Role>();
        }
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int IsDeleted { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
