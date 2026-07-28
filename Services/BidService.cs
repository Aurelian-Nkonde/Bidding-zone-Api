using bidding_zone_api.AppContext;
using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Dtos.Response;
using bidding_zone_api.Interfaces;
using bidding_zone_api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace bidding_zone_api.Services;

public class BidService: IBidService
{
    private readonly AppDbContext _context;
    private IItemsService _itemsService;
    private readonly ILogger<Bid> _logger;
    public BidService(AppDbContext contenxt, ILogger<Bid> logger, IItemsService itemService)
    {
        _context = contenxt;
        _logger = logger;
        _itemsService = itemService;
    }
    public async Task<BidResponseDto?> GetBid(Guid id)
    {
        _logger.LogInformation("Fetching bid with id: {id}", id);
        var bid = await _context.Bids.FindAsync(id);
        if(bid == null)
        {
            _logger.LogWarning("Bid with id {id} is not found", id);
            return null;
        }
        return BidResponseFormater(bid);
    }

    public async Task<IEnumerable<BidResponseDto>?> GetBids()
    {
        _logger.LogInformation("Fetching all bids");
        var bids = await _context.Bids.ToListAsync();
        _logger.LogInformation("Fetched {Count} bids", bids.Count);
        return bids.Select(bid => BidResponseFormater(bid));
    }

    public async Task<BidResponseDto> CreateBid(CreateBidDto bid)
    {
        _logger.LogInformation("Attempting to create a new bid for Item: {ItemId} by User: {UserId}", bid?.ItemId, bid?.UserId);
        if(bid == null){
            _logger.LogWarning("New bid data is null");
            throw new BadHttpRequestException("New bid data is null");
        };
        // if(!Enum.TryParse<BidStatus>(bid.Status, out var parsedStatus))
        // {
        //     _logger.LogWarning("Invalid bid status enum value: {Status}", bid.Status);
        //     throw new BadHttpRequestException("Error parsing bid status enum");
        // }
        var newBid = new Bid{
            Id = Guid.NewGuid(),
            ItemId = bid.ItemId,
            UserId = bid.UserId,
            Price = bid.Price,
            Status = BidStatus.Active,
            CreatedAt = DateTime.UtcNow
        };
        await _context.Bids.AddAsync(newBid);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Successfully created bid with ID: {BidId}", newBid.Id);
        //update item with the highest bidder
        var currentBidWinner = await GetHighestBidForItem(newBid.ItemId);
        if(currentBidWinner != null)
        {
            await _itemsService.UpdateCurrentWinner(newBid.ItemId, (Guid)currentBidWinner);
        }
        return BidResponseFormater(newBid);
    }
    
    public async Task<Guid?> GetHighestBidForItem(Guid itemId)
    {
        return await _context.Bids
            .AsNoTracking()
            .Where(bid => bid.ItemId == itemId)
            .OrderByDescending(bid => bid.Price)
            .Select(bid => (Guid?)bid.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<BidResponseDto>> GetUserBids(Guid id)
    {
        _logger.LogInformation("Fetching bids for user with id: {id}", id);
        var user = await _context.Users.FindAsync(id);
        if(user == null)
        {
            _logger.LogWarning("User with id {id} is not found", id);
            throw new BadHttpRequestException("user is not found");
        }
        var bids = await _context.Bids.Where(bid => bid.UserId == id).ToListAsync();
        _logger.LogInformation("Fetched {Count} bids for user {id}", bids.Count, id);
        return bids.Select(bid => BidResponseFormater(bid));
    }

    public async Task<bool?> DeleteBid(Guid id,Guid userId)
    {
        _logger.LogInformation("Attempting to delete bid {id} for user {userId}", id, userId);
        var bid = await _context.Bids.FindAsync(id);
        if(bid == null)
        {
            _logger.LogWarning("Bid with id {id} does not exist", id);
            throw new BadHttpRequestException("bid does not exist");
        }
        if(bid.UserId != userId)
        {
            _logger.LogWarning("User with id: {userId} is not bid {id} owner", userId, id);
            throw new BadHttpRequestException("User is not bid owner");
        }
        _context.Bids.Remove(bid);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Bid {id} is deleted", id);
        return true;
    }

    public async Task <BidResponseDto?> UpdateBid(UpdateBidDto bidData, Guid id)
    {
        _logger.LogInformation("Attempting to update bid {id}", id);
        if(bidData == null)
        {
            _logger.LogWarning("Bid data missing for bid {id}", id);
            return null;
        }
        if(!Enum.TryParse<BidStatus>(bidData.Status, out var parsedStatus))
        {
            _logger.LogWarning("Invalid bid status value: {Status}", bidData.Status);
            throw new BadHttpRequestException("Error parsing bid status value");
        }
        var bid = await _context.Bids.FindAsync(id);
        if(bid == null)
        {
            _logger.LogWarning("Bid with id {id} is not found", id);
            return null;
        }
        bid.Price = bidData.Price;
        bid.Status = parsedStatus;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Bid {id} is updated", id);
        return BidResponseFormater(bid);
    }
    
    public async Task<BidResponseDto?> ChangeBidStatus(string status, Guid id)
    {
        _logger.LogInformation("Attempting to change status of bid {id} to {Status}", id, status);
        if(!Enum.TryParse<BidStatus>(status, out var parsedStatus))
        {
            _logger.LogWarning("Invalid status enum value: {Status}", status);
            throw new BadHttpRequestException("Error parsing status value");
        }
        var bid = await _context.Bids.FindAsync(id);
        if(bid == null)
        {
            _logger.LogWarning("Bid with id {id} is not found", id);
            return null;
        }
        bid.Status = parsedStatus;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Bid {id} status is updated", id);
        return BidResponseFormater(bid);
    }

    public BidResponseDto BidResponseFormater(Bid bid)
    {
        return new BidResponseDto
        {
            Id = bid.Id,
            ItemId = bid.ItemId,
            UserId = bid.UserId,
            Price = bid.Price,
            Status = bid.Status,
            CreatedAt = bid.CreatedAt
        };
    }
}