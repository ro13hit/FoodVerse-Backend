using FoodDeliverySampleApplication.API;
using FoodDeliverySampleApplication.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodDeliverySampleApplication.Logic
{
    public interface IUserRepository
    {
        Task<ActionResult<UserResponse>> GetUserAsync(UserRequest request);
        Task<ActionResult<SignInResponse>>SignInAsync(SignInDTO request);
        Task<ActionResult<User>> SignUpAsync(UserDTO request);
        SignInResponse Refresh(RefreshTokenDTO RefreshCred);
        public string CheckEmailPhone(string value);
    }
}
