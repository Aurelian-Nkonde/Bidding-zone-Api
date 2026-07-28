using bidding_zone_api.Models;
using Microsoft.AspNetCore.Authorization;

namespace bidding_zone_api.Utils;

public class IsUserOwnerHandler: AuthorizationHandler<IsUserOwnerRequirement, User>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        IsUserOwnerRequirement requirement,
        User user)
    {
    var userId = context.User.FindFirst("sub")?.Value;
    if(userId == null)
        {
            return Task.CompletedTask;
        }
    if(!Guid.TryParse(userId, out var parsedId))
        {
            return Task.CompletedTask;
        }
    if(user.Id == parsedId)
        {
         context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}