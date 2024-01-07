using RestaurantAPI.Models.Dtos;

namespace RestaurantAPI.Interfaces
{
    public interface IAccountService
    {
        void AccountPost(UserCreateDto dto);
        string GenerateJwt(LoginDto dto);
    }
}
