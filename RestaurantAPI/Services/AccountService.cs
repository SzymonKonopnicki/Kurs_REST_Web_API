using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Entities;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Models.Dtos;

namespace RestaurantAPI.Services
{
    public class AccountService : IAccountService
    {
        private RestaurantDbContext _dbContext;
        private IPasswordHasher<User> _hasher;

        public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> hasher)
        {
            _dbContext = dbContext;
            _hasher = hasher;
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
    }
}
