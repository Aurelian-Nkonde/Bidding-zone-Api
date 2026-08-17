using bidding_zone_api.Interfaces;
using bidding_zone_api.Messages;
using bidding_zone_api.Models;
using MassTransit;

namespace bidding_zone_api.Consumers;

public class ItemExpiredConsumer : IConsumer<ItemExpiredMessage>
{
    private readonly IItemsService _itemsService;
    private readonly ILogger<ItemExpiredConsumer> _logger;

    public ItemExpiredConsumer(IItemsService itemsService, ILogger<ItemExpiredConsumer> logger)
    {
        _itemsService = itemsService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ItemExpiredMessage> context)
    {
        _logger.LogInformation("Marking item {ItemId} as expired", context.Message.ItemId);
        await _itemsService.UpdateItemStatus(nameof(ItemStatus.Expired), context.Message.ItemId);
    }
}
