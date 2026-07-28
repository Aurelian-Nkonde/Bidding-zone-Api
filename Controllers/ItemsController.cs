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
[Route("/api/{controller}")]
public class ItemsController: ControllerBase
{
    private readonly ILogger<Item> _logger;
    private readonly IValidator<UpdateItemDto> _updateItemValidator;
    private readonly IValidator<UpdateItemStatusDtos> _updateItemStatusValidator;
    private readonly IValidator<CreateItemDto> _createItemValidator;
    private readonly IItemsService _itemsService;
    public ItemsController(ILogger<Item> logger, IItemsService itemService,
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
        return Ok(await _itemsService.GetItems());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetItem([FromRoute] Guid id)
    {
        var result = await _itemsService.GetItem(id);
        if(result == null) return NotFound();
        return Ok(result);
    }

    [HttpPut("update/{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateItem([FromBody] UpdateItemDto item, [FromRoute] Guid id)
    {
        ValidationResult validationResult = await _updateItemValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed");
            return BadRequest();
        }
        try
        {
            var userId = "0be17b09-2ff6-4938-bdf3-c73b3d3b423a";
            if(!Guid.TryParse(userId, out Guid parsed))
            {
                return BadRequest();
            }
            var result = await _itemsService.UpdateItem(item, id, parsed);
            if(result == null) return NotFound();
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
        ValidationResult validationResult = await _updateItemStatusValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed");
            return BadRequest();
        }
        try
        {
            var userId = "0be17b09-2ff6-4938-bdf3-c73b3d3b423a";
            if(!Guid.TryParse(userId, out Guid parsed))
            {
                return BadRequest();
            }
            var result = await _itemsService.UpdateItemStatus(item.Status, id, parsed);
            if(result == null) return NotFound();
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
        ValidationResult validationResult = await _createItemValidator.ValidateAsync(item);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Invalid validation");
            return BadRequest();
        }
        try
        {
            var result = await _itemsService.AddItem(item);
            return CreatedAtAction(nameof(GetItem), new {id = result.Id}, result);
        }
        catch(BadHttpRequestException ex)
        {
            _logger.LogWarning("Error message: {ex}", ex.Message);
            return BadRequest();
        }
    }
}