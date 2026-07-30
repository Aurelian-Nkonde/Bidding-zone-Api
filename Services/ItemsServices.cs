using bidding_zone_api.AppContext;
using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Dtos.Response;
using bidding_zone_api.Interfaces;
using bidding_zone_api.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace bidding_zone_api.Services;

public class ItemService: IItemsService
{
    private readonly AppDbContext _context;
    private readonly ILogger<Item> _logger;
    public ItemService(AppDbContext contenxt, ILogger<Item> logger)
    {
        _context = contenxt;
        _logger = logger;
    }

    public async Task<ItemResponseDto> AddItem(CreateItemDto data)
    {
        _logger.LogInformation("Attempting to add a new item for User: {UserId}", data.UserId);

        if (await UserExist(data.UserId) == false)
        {
            throw new BadHttpRequestException("User does not exist.");
        }

        if (!Enum.TryParse<Bidding_times>(data.EndTimer, out var parsedTimer))
        {
            _logger.LogWarning("Invalid endtimer enum value: {EndTimer}", data.EndTimer);
            throw new BadHttpRequestException("Invalid endtimer enum value");
        }
        var timerMinutes = AddTimer(parsedTimer) ?? throw new BadHttpRequestException("Invalid endtimer calculation value");

        var newItem = new Item
        {
            Id = Guid.NewGuid(), 
            UserId = data.UserId,
            Title = data.Title,
            Description = data.Description,
            StartingPrice = data.StartingPrice,
            Status = ItemStatus.Active, 
            CreatedAt = DateTime.UtcNow,
            EndTimer = DateTime.UtcNow.AddMinutes((double)timerMinutes) 
        };

        await _context.Items.AddAsync(newItem);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Successfully created item with ID: {ItemId}", newItem.Id);

        return MapToResponseDto(newItem);
    }   

    public async Task<ItemResponseDto?> GetItem(Guid id)
    {
        _logger.LogInformation("Fetching item with id: {id}", id);
        var item = await _context.Items.FindAsync(id);
        if(item == null)
        {
            _logger.LogWarning("Item with id {id} is not found", id);
            return null;
        }
        return MapToResponseDto(item);
    }

    public async Task<IEnumerable<ItemResponseDto>> GetItems()
    {
        _logger.LogInformation("Fetching all items");
        var items = await _context.Items.ToListAsync();
        _logger.LogInformation("Fetched {Count} items", items.Count);
        return items.Select(item => MapToResponseDto(item));
    }

    public async Task<bool?> UpdateItem(UpdateItemDto itemData, Guid id)
    {
        _logger.LogInformation("checking if item with id: {id} exist", id);
        var item = await _context.Items.FindAsync(id);
        if(item == null)
        {
            return null;
        }

        if(itemData == null || itemData!.EndTimer == null)
        {
            _logger.LogWarning("Item data or endTimer missing for item {id}", id);
            throw new BadHttpRequestException("item data and endTimer are required");
        }

        if(!Enum.TryParse<Bidding_times>(itemData.EndTimer, out var parsedTimer))
        {
            _logger.LogWarning("Invalid endtimer enum value: {EndTimer}", itemData.EndTimer);
            throw new BadHttpRequestException("Invalid endtimer enum value");
        }

        var timer = AddTimer(parsedTimer) ?? throw new BadHttpRequestException("Invalid endtimer value");
        item.Description = itemData.Description;
        item.Title = itemData.Title;
        item.StartingPrice = itemData.StartingPrice;
        item.EndTimer = DateTime.UtcNow.AddMinutes((double)timer);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Item {id} is updated", id);
        return true;
    }

    public async Task<ItemResponseDto?> UpdateItemStatus(string status, Guid id)
    {

        _logger.LogInformation("checking if item with id: {id} exist", id);
        var item = await _context.Items.FindAsync(id);
        if(item == null)
        {
            return null;
        }
    
        if(!Enum.TryParse<ItemStatus>(status, out var parsedStatus))
        {
            _logger.LogWarning("Invalid status enum value: {Status}", status);
            throw new BadHttpRequestException("Invalid status enum value");
        }
        item.Status = parsedStatus;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Item status is updated");
        return MapToResponseDto(item);
    }

    public async Task<IEnumerable<ItemResponseDto>?> GetUserItems(Guid id)
    {
        _logger.LogInformation("looking for a user with id: {id}", id);
        var user = await _context.Users.FindAsync(id);
        if(user == null)
        {
            _logger.LogWarning("user with id: {id} is not found", id);
            return null;
        }
        var result = await _context.Items.Where(item => item.UserId == id).ToListAsync();
        return result.Select(item => MapToResponseDto(item));
    }

    async Task<bool> ItemExist(Guid id)
    {
        var item = await _context.Items.FindAsync(id);
        if(item == null)
        {
            _logger.LogWarning("Item with id {id} is not found", id);
            return false;
        }
        return true;
    }

    async Task<bool> UserExist(Guid id)
    {
        var item = await _context.Users.FindAsync(id);
        if(item == null)
        {
            _logger.LogWarning("User with id {id} is not found", id);
            return false;
        }
        return true;
    }

    public int? AddTimer(Bidding_times times)
    {
        return times switch
        {
            Bidding_times.Hours_1 => 60,
            Bidding_times.Minutes_5 => 5,
            Bidding_times.Minutes_10 => 10,
            Bidding_times.Minutes_30 => 30,
            _ => null
        };
    }

   public async Task<ItemResponseDto?> UpdateCurrentWinner(Guid id, Guid bid)
    {
        var item = await _context.Items.FindAsync(id);
        if(item == null) return null;
        item.WinnerUserId = bid;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Current highest bid is updated");
        return MapToResponseDto(item);
    }


    public async Task<int> GetItemsCount()
    {
        _logger.LogInformation("Counting all items");
        return await _context.Items.CountAsync();
    }

    public async Task<int> GetUserItemsCount(Guid id)
    {
        _logger.LogInformation("Counting items for user {id}", id);
        return await _context.Items.Where(item => item.UserId == id).CountAsync();
    }

    private static ItemResponseDto MapToResponseDto(Item item)
    {
        return new ItemResponseDto
        {
            Id = item.Id,
            UserId = item.UserId,
            EndTimer = item.EndTimer,
            StartingPrice = item.StartingPrice,
            WinnerUserId = item.WinnerUserId,
            Title = item.Title,
            Description = item.Description,
            Status = item.Status,
            CreatedAt = item.CreatedAt
        };
    }
}