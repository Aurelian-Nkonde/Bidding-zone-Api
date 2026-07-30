using Microsoft.AspNetCore.Authorization;

namespace bidding_zone_api.Utils;

public class IsUserOwnerHandler: AuthorizationHandler<IsUserOwnerRequirement, Guid>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        IsUserOwnerRequirement requirement,
        Guid resourceId)
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
    if(resourceId == parsedId)
        {
         context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}