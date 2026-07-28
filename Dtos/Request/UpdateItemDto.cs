namespace bidding_zone_api.Dtos.Request;

public class UpdateItemDto
{
    public string EndTimer {get;set;} = string.Empty;
    public decimal StartingPrice {get;set;}
    public string Title {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    
}