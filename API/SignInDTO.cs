using System.ComponentModel.DataAnnotations;

namespace FoodDeliverySampleApplication.API
{
    public class SignInDTO
    {
        [EmailAddress, Required, MaxLength(40)]
        public string Email { get; set; }
        [Required, RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\w\d\s:])([^\s]){8,16}$")]
        public string Password { get; set; }
    }
}
