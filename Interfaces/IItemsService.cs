using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Dtos.Response;
using bidding_zone_api.Models;

namespace bidding_zone_api.Interfaces;

public interface IItemsService
{
    Task<ItemResponseDto> AddItem(CreateItemDto data);
    Task<ItemResponseDto?> GetItem(Guid id);
    Task<PagedResult<ItemResponseDto>> GetItems(int page, ItemStatus? status = null);
    Task<bool?> UpdateItem(UpdateItemDto itemData, Guid id);
    Task<ItemResponseDto?> UpdateItemStatus(string status, Guid id);
    Task<IEnumerable<ItemResponseDto>?> GetUserItems(Guid id, ItemStatus? status = null);
    Task<ItemResponseDto?> UpdateCurrentWinner(Guid id, Guid bid);
    Task<int> GetItemsCount();
    Task<int> GetUserItemsCount(Guid id);
    Task<bool?> DeleteItem(Guid id);
}