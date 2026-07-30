using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Dtos.Response;
using bidding_zone_api.Models;

namespace bidding_zone_api.Interfaces;

public interface IItemsService
{
    Task<ItemResponseDto> AddItem(CreateItemDto data);
    Task<ItemResponseDto?> GetItem(Guid id);
    Task<IEnumerable<ItemResponseDto>> GetItems();
    Task<bool?> UpdateItem(UpdateItemDto itemData, Guid id);
    Task<ItemResponseDto?> UpdateItemStatus(string status, Guid id);
    Task<IEnumerable<ItemResponseDto>?> GetUserItems(Guid id);
    Task<ItemResponseDto?> UpdateCurrentWinner(Guid id, Guid bid);
    Task<int> GetItemsCount();
    Task<int> GetUserItemsCount(Guid id);
}