using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Dtos.Response;
using bidding_zone_api.Models;
using bidding_zone_api.Services;
using bidding_zone_api.Interfaces;

using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.Results;
using bidding_zone_api.Dtos.Validators;

namespace bidding_zone_api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ItemsController: ControllerBase
{
    private readonly ILogger<ItemsController> _logger;
    private readonly IValidator<UpdateItemDto> _updateItemValidator;
    private readonly IValidator<UpdateItemStatusDtos> _updateItemStatusValidator;
    private readonly IValidator<CreateItemDto> _createItemValidator;
    private readonly IItemsService _itemsService;
    public ItemsController(ILogger<ItemsController> logger, IItemsService itemService,
    IValidator<UpdateItemDto> updateItemValidator,
    IValidator<UpdateItemStatusDtos> updateItemStatusValidator,
    IValidator<CreateItemDto> createItemValidator
    )
    {
        _logger = logger;
        _itemsService = itemService;
        _updateItemValidator  = updateItemValidator;
        _updateItemStatusValidator = updateItemStatusValidator;
        _createItemValidator = createItemValidator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetItems()
    {
        _logger.LogInformation("Fetching all items");
        return Ok(await _itemsService.GetItems());
    }

    [HttpGet("count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetItemsCount()
    {
        _logger.LogInformation("Fetching total item count");
        var count = await _itemsService.GetItemsCount();
        return Ok(count);
    }

    [HttpGet("user/{id}/count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetUserItemsCount([FromRoute] Guid id)
    {
        _logger.LogInformation("Fetching item count for user {UserId}", id);
        var count = await _itemsService.GetUserItemsCount(id);
        return Ok(count);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetItem([FromRoute] Guid id)
    {
        _logger.LogInformation("Fetching item {ItemId}", id);
        var result = await _itemsService.GetItem(id);
        if(result == null)
        {
            _logger.LogWarning("Item {ItemId} not found", id);
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPut("update/{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateItem([FromBody] UpdateItemDto item, [FromRoute] Guid id)
    {
        _logger.LogInformation("Attempting to update item {ItemId}", id);
        ValidationResult validationResult = await _updateItemValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for item {ItemId} update", id);
            return BadRequest();
        }
        try
        {

            var result = await _itemsService.UpdateItem(item, id);
            if(result == null)
            {
                _logger.LogWarning("Item {ItemId} not found for update", id);
                return NotFound();
            }
            _logger.LogInformation("Item {ItemId} updated successfully", id);
            return NoContent();
        }
        catch(BadHttpRequestException ex)
        {
            _logger.LogWarning("Error message: {ex}", ex.Message);
            return BadRequest();
        }
    }

    [HttpPut("update/{id}/status")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateItemStatus([FromBody] UpdateItemStatusDtos item, [FromRoute] Guid id)
    {
        _logger.LogInformation("Attempting to update status of item {ItemId}", id);
        ValidationResult validationResult = await _updateItemStatusValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for item {ItemId} status update", id);
            return BadRequest();
        }
        try
        {

            var result = await _itemsService.UpdateItemStatus(item.Status, id);
            if(result == null)
            {
                _logger.LogWarning("Item {ItemId} not found for status update", id);
                return NotFound();
            }
            _logger.LogInformation("Item {ItemId} status updated successfully", id);
            return NoContent();
        }
        catch(BadHttpRequestException ex)
        {
            _logger.LogWarning("Error message: {ex}", ex.Message);
            return BadRequest();
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> AddItem([FromBody] CreateItemDto item)
    {
        _logger.LogInformation("Attempting to add item titled {Title} for user {UserId}", item.Title, item.UserId);
        ValidationResult validationResult = await _createItemValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for new item titled {Title}", item.Title);
            return BadRequest();
        }
        try
        {
            var result = await _itemsService.AddItem(item);
            _logger.LogInformation("Item {ItemId} created successfully", result.Id);
            return CreatedAtAction(nameof(GetItem), new {id = result.Id}, result);
        }
        catch(BadHttpRequestException ex)
        {
            _logger.LogWarning("Error message: {ex}", ex.Message);
            return BadRequest();
        }
    }
}