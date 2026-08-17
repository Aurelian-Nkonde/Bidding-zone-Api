namespace bidding_zone_api.Models;

public enum ItemStatus
{
    Active,
    Sold,
    Canceled,
    Expired
}

public class Item
{
    public Guid Id {get;set;}
    public Guid UserId {get;set;}
    public DateTime EndTimer {get;set;}
    public decimal StartingPrice {get;set;}
    public Guid? WinnerUserId {get;set;} = Guid.Empty;
    public string Title {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    public string? Image {get;set;} = string.Empty;
    public ItemStatus Status {get;set;}
    public DateTime? CreatedAt {get;set;}
    public DateTime? UpdatedAt {get;set;}
}