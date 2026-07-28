namespace bidding_zone_api.Dtos.Request;

public class UpdateBidDto
{
    public decimal Price {get;set;}
    public string Status {get;set;} = string.Empty;
}