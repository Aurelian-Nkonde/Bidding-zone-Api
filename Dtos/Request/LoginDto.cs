using bidding_zone_api.Models;

namespace bidding_zone_api.Dtos.Request;

public class LoginDto
{
    public string Password {get;set;} = string.Empty;
    public string Email {get;set;} = string.Empty;
}