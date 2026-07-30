using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Dtos.Response;

namespace bidding_zone_api.Interfaces;

public interface IBidService
{
    Task<BidResponseDto?> GetBid(Guid id);
    Task<IEnumerable<BidResponseDto>?> GetBids();
    Task<BidResponseDto> CreateBid(CreateBidDto data);
    Task<IEnumerable<BidResponseDto>> GetUserBids(Guid id);
    Task<bool?> DeleteBid(Guid id,Guid userId);
    Task <BidResponseDto?> UpdateBid(UpdateBidDto bid, Guid id); 
    Task<BidResponseDto?> ChangeBidStatus(string status, Guid id);
    Task<int> GetBidsCount();
    Task<int> GetUserBidsCount(Guid id);
}