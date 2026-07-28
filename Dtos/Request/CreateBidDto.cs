namespace bidding_zone_api.Dtos.Request;

public class CreateBidDto
{
    public Guid ItemId {get;set;}
    public Guid UserId {get;set;}
    public decimal Price {get;set;}
    // public string Status {get;set;} = string.Empty;
}