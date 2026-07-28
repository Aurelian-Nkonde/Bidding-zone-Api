namespace bidding_zone_api.Dtos.Request;

public class CreateItemDto
{
    public Guid UserId {get;set;}
    public string EndTimer {get;set;} = string.Empty;
    public decimal StartingPrice {get;set;}
    public string Title {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    public string? Image {get;set;} = string.Empty;
}

public enum Bidding_times
{
    Minutes_5,
    Minutes_10,
    Minutes_30,
    Hours_1,
}