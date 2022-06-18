using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliverySampleApplication.API
{
    public class UserDTO
    {
        [Required, MaxLength(40)]
        public string FirstName { get; set; }
        [Required, MaxLength(40)]
        public string LastName { get; set; }
        [Required,EmailAddress, MaxLength(40)]
        public string Email { get; set; }
        [Required, RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\w\d\s:])([^\s]){8,16}$")]
        public string Password { get; set; }
        [Required, MaxLength(10)]
        public string Phone { get; set; }
        [Required, MaxLength(100)]
        public string Address { get; set; }
    }
}
