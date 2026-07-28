using bidding_zone_api.Models;

namespace bidding_zone_api.Dtos.Request;

public class UpdateUserDto
{
    public string FirstName {get;set;} = string.Empty;
    public string LastName {get;set;} = string.Empty;
    public string Gender {get;set;} = string.Empty;
    public string Email {get;set;} = string.Empty;
    public AddressDto? Address {get;set;} = null;
}