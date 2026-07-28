using bidding_zone_api.Models;

namespace bidding_zone_api.Dtos.Request;

public class AddressDto
{
    public string StreetName {get;set;} = string.Empty;
    public string Surbub {get;set;} = string.Empty;
    public int HouseNumber {get;set;}
    public string Province {get;set;} = string.Empty;
    
}