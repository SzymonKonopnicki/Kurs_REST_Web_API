using System.Security.Claims;
using RestaurantAPI.Interfaces;

namespace RestaurantAPI.Services
{
    public class UserContextServic : IUserContextServic
    {
        private IHttpContextAccessor _httpContextAccessor;

        public UserContextServic(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public int? GetUserId => User is null ? null : (int?)int.Parse(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }
}
