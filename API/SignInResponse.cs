using System.ComponentModel.DataAnnotations;

namespace FoodDeliverySampleApplication.API
{
    public class SignInResponse
    {
        public string message { get; set;}
        public string Email { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
