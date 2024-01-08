using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Security.Claims;

namespace RestaurantAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Restaurant>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Restaurant restaurant)
        {
            if (requirement.ResourceOperation == ResourceOperation.Create || requirement.ResourceOperation == ResourceOperation.Read)
                context.Succeed(requirement);

            var userId = context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            if (restaurant.CreatedById == int.Parse(userId))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
