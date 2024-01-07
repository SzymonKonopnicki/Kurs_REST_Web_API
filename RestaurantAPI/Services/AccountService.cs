using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web.LayoutRenderers;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantAPI.Services
{
    public class AccountService : IAccountService
    {
        private RestaurantDbContext _dbContext;
        private IPasswordHasher<User> _hasher;
        private AuthenticationSettings _autenticationSettings;

        public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> hasher, AuthenticationSettings autenticationSettings)
        {
            _dbContext = dbContext;
            _hasher = hasher;
            _autenticationSettings = autenticationSettings;
        }
        public void AccountPost(UserCreateDto dto)
        {
            User user = new User()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Email = dto.Email,
                Nationality = dto.Nationality,
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);
            user.RoleId = 3;

            _dbContext.AddRange(user);
            _dbContext.SaveChanges();
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _dbContext.Users
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Email == dto.Email);
            if (user is null)
                throw new BadRequestException("Bad username or passwor.");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new BadRequestException("Bad username or passwor.");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")),
                new Claim("Nationality", user.Nationality)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(_autenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var exp = DateTime.Now.AddDays(_autenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_autenticationSettings.JwtIssuer,
                _autenticationSettings.JwtIssuer,
                claims,
                expires: exp,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}
