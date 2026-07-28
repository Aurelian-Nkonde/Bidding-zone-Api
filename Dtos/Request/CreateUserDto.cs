using bidding_zone_api.Models;

namespace bidding_zone_api.Dtos.Request;

public class CreateUserDto
{
    public string FirstName {get;set;} = string.Empty;
    public string LastName {get;set;} = string.Empty;
    public string Password {get;set;} = string.Empty;
    public string Gender {get;set;} = string.Empty;
    public string Email {get;set;} = string.Empty;
    public AddressDto? Address {get;set;} = null;
}