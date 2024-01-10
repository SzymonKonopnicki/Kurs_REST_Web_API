using System.Security.Claims;

namespace RestaurantAPI.Interfaces
{
    public interface IUserContextServic
    {
        int? GetUserId { get; }
        ClaimsPrincipal User { get; }
    }
}