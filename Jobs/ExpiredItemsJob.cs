using bidding_zone_api.AppContext;
using bidding_zone_api.Messages;
using bidding_zone_api.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace bidding_zone_api.Jobs;

public interface IExpiredItemsJob
{
    Task CheckExpiredItemsAsync();
}

public class ExpiredItemsJob : IExpiredItemsJob
{
    private readonly AppDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ExpiredItemsJob> _logger;

    public ExpiredItemsJob(AppDbContext context, IPublishEndpoint publishEndpoint, ILogger<ExpiredItemsJob> logger)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task CheckExpiredItemsAsync()
    {
        var expiredItems = await _context.Items
            .Where(item => item.Status == ItemStatus.Active && item.EndTimer <= DateTime.UtcNow)
            .ToListAsync();

        _logger.LogInformation("Found {Count} expired items", expiredItems.Count);

        foreach (var item in expiredItems)
        {
            await _publishEndpoint.Publish(new ItemExpiredMessage(item.Id));
        }
    }
}
