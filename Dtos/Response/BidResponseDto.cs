using bidding_zone_api.Models;

namespace bidding_zone_api.Dtos.Response;

public class BidResponseDto
{
    public Guid Id {get;set;}
    public Guid ItemId {get;set;}
    public Guid UserId {get;set;}
    public decimal Price {get;set;}
    public BidStatus Status {get;set;}
    public DateTime? CreatedAt {get;set;}
}
