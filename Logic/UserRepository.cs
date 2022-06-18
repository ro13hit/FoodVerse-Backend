using FoodDeliverySampleApplication.API;
using FoodDeliverySampleApplication.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System;

namespace FoodDeliverySampleApplication.Logic
{
    public class UserRepository : IUserRepository
    {
        private readonly FoodDeliveryContext _context;
        private readonly IConfiguration _configuration;
        public static IDictionary<string, string> UserRefreshTokens = new Dictionary<string, string>();
        public UserRepository(FoodDeliveryContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;  
        }

        #region Private Methods
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateJwtToken(string Email)
        {
            var TokenHandler = new JwtSecurityTokenHandler();
            var TokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim (ClaimTypes.Name, Email)
                }),
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(TokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = TokenHandler.CreateToken(TokenDescriptor);
            return TokenHandler.WriteToken(token);
        }

        private SignInResponse GenerateJwtToken(Claim[] claims, string Email)
        {
            var jwtSecurityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(5),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256Signature
                )
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return new SignInResponse
            {
                Token = token,
                RefreshToken = UserRefreshTokens[Email]
            };
        }
        #endregion
        public SignInResponse Refresh(RefreshTokenDTO RefreshCred)
        {
            var TokenHandler = new JwtSecurityTokenHandler();
            var Principal = TokenHandler.ValidateToken(RefreshCred.Token,
                new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
                }, out SecurityToken validatedToken);
            var JwtToken = validatedToken as JwtSecurityToken;
            if(JwtToken==null || !JwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token passed!");
            }

            var userName = Principal.Identity.Name;
            if(RefreshCred.RefreshToken!=UserRefreshTokens[userName])
            {
                throw new SecurityTokenException("Invalid refresh token passed!");
            }
            return GenerateJwtToken(Principal.Claims.ToArray(), userName);
        }

        public async Task<ActionResult<UserResponse>> GetUserAsync(UserRequest request)
        {
            var user = await _context.User.FirstOrDefaultAsync(q => q.Email == request.Email && q.IsDeleted != 1);
            if (user != null)
            {
                UserResponse result = new();
                result.FirstName = user.FirstName;
                result.Email = user.Email;
                result.Address = user.Address;
                result.Phone = user.Phone;
                result.UserId = user.UserId;
                return result;
            }
            return null;
        }

        public async Task<ActionResult<SignInResponse>> SignInAsync(SignInDTO request)
        {
            var newUser = await _context.User.FirstOrDefaultAsync(u => u.Email == request.Email && u.IsDeleted==0);
            if (newUser != null && VerifyPasswordHash(request.Password, newUser.PasswordHash, newUser.PasswordSalt))
            {
                
                var token = GenerateJwtToken(newUser.Email);
                var RefreshToken = GenerateRefreshToken();
                if(UserRefreshTokens.ContainsKey(newUser.Email))
                {
                    UserRefreshTokens[newUser.Email] = RefreshToken;
                }
                else
                {
                    UserRefreshTokens.Add(newUser.Email, RefreshToken);
                }
                return new SignInResponse
                {
                    Token = token,
                    RefreshToken = RefreshToken
                };
            }
            return null;
        }

        public async Task<ActionResult<User>> SignUpAsync(UserDTO request)
        {
            User newUser = new();
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            newUser.Email = request.Email;
            newUser.FirstName = request.FirstName;
            newUser.LastName = request.LastName;
            newUser.Phone = request.Phone;
            newUser.Address = request.Address;
            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;
            _context.User.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public string CheckEmailPhone(string value)
        {
            var EmailCheck =  _context.User.FirstOrDefault(u => u.Email == value);
            var PhoneCheck =  _context.User.FirstOrDefault(u => u.Phone == value);
            if (EmailCheck != null)
            {
                return "email";
            }
            else if (PhoneCheck != null)
            {
                return "phone";
            }
            return "unique";
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
