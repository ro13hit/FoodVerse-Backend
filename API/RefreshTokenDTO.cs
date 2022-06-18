using System.ComponentModel.DataAnnotations;

namespace FoodDeliverySampleApplication.API
{
    public class RefreshTokenDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
