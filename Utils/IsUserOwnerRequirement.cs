using Microsoft.AspNetCore.Authorization;

namespace bidding_zone_api.Utils;

public class IsUserOwnerRequirement: IAuthorizationRequirement
{
}