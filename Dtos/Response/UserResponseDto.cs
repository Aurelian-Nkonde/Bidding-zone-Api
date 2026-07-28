using bidding_zone_api.Models;

namespace bidding_zone_api.Dtos.Response;

public class UserResponseDto
{
    public Guid Id {get;set;}
    public string FirstName {get;set;} = string.Empty;
    public string LastName {get;set;} = string.Empty;
    public Gender Gender {get;set;}
    public string Email {get;set;} = string.Empty;
    public Address? Address {get;set;} = null;
    public DateTime? CreatedAt {get;set;}
    public DateTime? UpdatedAt {get;set;}
    public Role Role {get;set;}
}