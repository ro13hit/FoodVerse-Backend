using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Linq;
using FoodDeliverySampleApplication.DAL;
using FoodDeliverySampleApplication.API;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDeliverySampleApplication.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace FoodDeliverySampleApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("GetUser")]
        public async Task<ActionResult<UserResponse>> GetUserByEmail(UserRequest request)
        {
            _logger.LogInformation("Getting all users present");
            var user =  await _userRepository.GetUserAsync(request);
            if(user!=null)
            {
                return Ok(user);
            }
            return NotFound("User doesn't exists for this email");
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<ActionResult<SignInResponse>> SignIn(SignInDTO request)
        {
            _logger.LogInformation("Sign in of user");
            var token = await _userRepository.SignInAsync(request);
            if (token != null )
            {
                var message = "Login Successfull!";
                _logger.LogInformation(message);
                return Ok(new 
                { 
                    message,
                    request.Email,
                    token
                });
            }
            _logger.LogWarning("User not found for {Email}", request.Email);
            return NotFound("Check your credentials and try again!");
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<ActionResult<User>> SignUp(UserDTO request)
        {
            _logger.LogInformation("Sign up for new user");
            var user = await _userRepository.SignUpAsync(request);
            if(user != null)
            {
                var message = "Signup Successfull!";
                _logger.LogInformation(message);
                return Ok(new
                {
                    message,
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Address
                });
            }
            _logger.LogWarning("Couldn't sign up some fields are incorrect for {Email}", request.Email);
            return BadRequest("Provide complete details for signup process!");
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public ActionResult<RefreshTokenDTO> RefreshTokens(RefreshTokenDTO RefreshCred)
        {
            _logger.LogInformation("Trying to generate a new token for user to refresh");
            var token = _userRepository.Refresh(RefreshCred);
            if(token==null)
            {
                _logger.LogWarning("Couldn't generate new token for user");
                return Unauthorized();
            }
            _logger.LogInformation("New token generated successfully");
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("UniqueCheck")]
        public async Task<ActionResult<string>> UniqueCheck(string value)
        {
            var check =  _userRepository.CheckEmailPhone(value);
            return Ok(new
            {
                check
            });
        }
    }
}
