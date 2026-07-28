namespace bidding_zone_api.Models;

public enum BidStatus
{
    Rejected,
    Accepted,
    Active,
    Canceled,
    Closed,
}

public class Bid
{
    public Guid Id {get;set;}
    public Guid ItemId {get;set;}
    public Guid UserId {get;set;}
    public decimal Price {get;set;}
    public BidStatus Status {get;set;}
    public DateTime? CreatedAt {get;set;}
    public DateTime? UpdatedAt {get;set;}
}