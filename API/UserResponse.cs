using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliverySampleApplication.API
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
         public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
