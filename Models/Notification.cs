namespace bidding_zone_api.Models;

public enum NotificationTitle
{
    Bid_is_accepted,
    Bid_is_rejected,
    New_bid_is_received,
    Your_bid_won,
    Your_bid_lost,
    Your_item_is_liked,
}

public class Notification
{
    public Guid Id {get;set;}
    public Guid UserId {get;set;}
    public NotificationTitle Title {get;set;}
    public Guid Message {get;set;}
    public bool IsRead {get;set;}
    public DateTime? CreatedAt {get;set;}
    public DateTime? UpdatedAt {get;set;}

}