using System.IdentityModel.Tokens.Jwt;
using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Dtos.Response;
using bidding_zone_api.Interfaces;
using bidding_zone_api.Models;
using bidding_zone_api.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bidding_zone_api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class BidsController: ControllerBase
{
    private readonly IBidService _bidService;
    private readonly ILogger<BidsController> _bidLogger;
    private readonly IValidator<CreateBidDto> _createBidValidator;
    private readonly IValidator<UpdateBidDto> _updateBidValidator;
    private readonly IValidator<UpdateBidStatusDto> _updateBidStatusValidator;

    public BidsController(IBidService service, ILogger<BidsController> bidLogger, 
        IValidator<CreateBidDto> createBidValidator,
        IValidator<UpdateBidDto> updateBidValidator,
        IValidator<UpdateBidStatusDto> updateBidStatusValidator
    )
    {
        _bidService = service;
        _bidLogger = bidLogger;
        _createBidValidator = createBidValidator;
        _updateBidStatusValidator = updateBidStatusValidator;
        _updateBidValidator = updateBidValidator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<BidResponseDto>>> GetBids([FromQuery] int page)
    {
        _bidLogger.LogInformation("Fetching all bids");
        var bids = await _bidService.GetBids(page);
        return Ok(bids);
    }

    [HttpGet("count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetBidsCount()
    {
        _bidLogger.LogInformation("Fetching total bid count");
        var count = await _bidService.GetBidsCount();
        return Ok(count);
    }


    [HttpGet("user/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<BidResponseDto>>> GetUserBids([FromRoute] Guid id)
    {
        _bidLogger.LogInformation("Fetching bids for user {UserId}", id);
       try
        {
            var bids = await _bidService.GetUserBids(id);
            if(bids == null)
            {
                _bidLogger.LogWarning("No bids found for user {UserId}", id);
                return NotFound();
            }
            return Ok(bids);
        }catch(BadHttpRequestException ex)
        {
            _bidLogger.LogWarning("Error {ex}", ex.Message);
            return BadRequest();
        }
    }

    [HttpGet("user/{id}/count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetUserBidsCount([FromRoute] Guid id)
    {
        _bidLogger.LogInformation("Fetching bid count for user {UserId}", id);
        var count = await _bidService.GetUserBidsCount(id);
        return Ok(count);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BidResponseDto>> GetABid([FromRoute]Guid id)
    {
        _bidLogger.LogInformation("Fetching bid {BidId}", id);
        var bid = await _bidService.GetBid(id);
        if(bid == null)
        {
            _bidLogger.LogWarning("Bid {BidId} not found", id);
            return NotFound();
        }
        return Ok(bid);
    }

    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteBid([FromRoute] Guid id)
    {
        var userIdClaim = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return BadRequest();
        }
        var isAdmin = User.IsInRole("Admin");

        _bidLogger.LogInformation("Attempting to delete bid {BidId} for user {UserId}", id, userId);
        try
        {
            var bid = await _bidService.DeleteBid(id, userId, isAdmin);
            if(bid == null)
            {
                _bidLogger.LogWarning("Bid {BidId} not found for deletion", id);
                return NotFound();
            }
            _bidLogger.LogInformation("Bid {BidId} deleted successfully", id);
            return NoContent();
        }catch(BadHttpRequestException ex)
        {
            _bidLogger.LogWarning("Error {ex}", ex.Message);
            return BadRequest();
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddABid([FromBody] CreateBidDto data)
    {
        ValidationResult validationResult = await _createBidValidator.ValidateAsync(data);
        if(!validationResult.IsValid)
        {
            _bidLogger.LogWarning("Validation failed for new bid on item {ItemId}", data.ItemId);
            return BadRequest();
        }
        _bidLogger.LogInformation("Creating bid for item {ItemId} by user {UserId} at price {Price}", data.ItemId, data.UserId, data.Price);
        try
        {
            var bid = await _bidService.CreateBid(data);
            _bidLogger.LogInformation("Bid {BidId} created successfully", bid.Id);
            return CreatedAtAction(nameof(GetABid), new {id = bid.Id}, bid);
        }catch(BadHttpRequestException ex)
        {
            _bidLogger.LogWarning("Error {ex}", ex.Message);
            return BadRequest();
        }
    }

    [HttpPut("status/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ChangeBidStatus([FromBody] UpdateBidStatusDto data, [FromRoute] Guid id)
    {
        ValidationResult validationResult = await _updateBidStatusValidator.ValidateAsync(data);
        if(!validationResult.IsValid)
        {
            _bidLogger.LogWarning("Validation failed for status update on bid {BidId}", id);
            return BadRequest();
        }

        _bidLogger.LogInformation("Changing status of bid {BidId} to {Status}", id, data.Status);
        try
        {
            var bid = await _bidService.ChangeBidStatus(data.Status, id);
            if(bid == null)
            {
                _bidLogger.LogWarning("Bid {BidId} not found for status update", id);
                return NotFound();
            }
            _bidLogger.LogInformation("Bid {BidId} status updated successfully", id);
            return NoContent();
        }catch(BadHttpRequestException ex)
        {
            _bidLogger.LogWarning("Error {ex}", ex.Message);
            return BadRequest();
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateABid([FromBody] UpdateBidDto data, [FromRoute] Guid id)
    {
        ValidationResult validationResult = await _updateBidValidator.ValidateAsync(data);
        if(!validationResult.IsValid)
        {
            return BadRequest();
        }

        _bidLogger.LogInformation("Updating bid with id {id}",id);
        try
        {
            var bid = await _bidService.UpdateBid(data, id);
            if(bid == null)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetABid), new {id = bid.Id}, bid);
        }catch(BadHttpRequestException ex)
        {
            _bidLogger.LogWarning("Error {ex}", ex.Message);
            return BadRequest();
        }
    }
}