namespace bidding_zone_api.Models;

public enum Gender
{
    Male,
    Female
}

public enum Role
{
    Admin,
    User
}

public class User
{
    public Guid Id {get;set;}
    public string FirstName {get;set;} = string.Empty;
    public string LastName {get;set;} = string.Empty;
    public string Password {get;set;} = string.Empty;
    public Gender Gender {get;set;}
    public string Email {get;set;} = string.Empty;
    public Address? Address {get;set;} = null;
    public DateTime? CreatedAt {get;set;}
    public DateTime? UpdatedAt {get;set;}
    public Role Role {get;set;}
}

public enum Province
{
    Western_cape,
    Northen_cape,
    Mpumalanga,
    Limpopo,
    Gauteng,
    Kwazulu_natal,
    North_west,
    Eastern_cape,
}

public class Address
{
    public string StreetName {get;set;} = string.Empty;
    public string Surbub {get;set;} = string.Empty;
    public int HouseNumber {get;set;}
    public Province Province {get;set;}
    
}