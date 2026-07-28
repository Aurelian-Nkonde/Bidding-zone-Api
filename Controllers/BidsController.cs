using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Dtos.Response;
using bidding_zone_api.Interfaces;
using bidding_zone_api.Models;
using bidding_zone_api.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace bidding_zone_api.Controllers;

[ApiController]
[Route("/api/{controller}")]
public class BidsController: ControllerBase
{
    private readonly IBidService _bidService;
    private readonly ILogger<Bid> _bidLogger;
    private readonly IValidator<CreateBidDto> _createBidValidator;
    private readonly IValidator<UpdateBidDto> _updateBidValidator;
    private readonly IValidator<UpdateBidStatusDto> _updateBidStatusValidator;

    public BidsController(IBidService service, ILogger<Bid> bidLogger, 
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
    public async Task<ActionResult<IEnumerable<BidResponseDto>>> GetBids()
    {
        var bids = await _bidService.GetBids();
        return Ok(bids);
    }

    [HttpGet("user/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<BidResponseDto>>> GetUserBids([FromRoute] Guid id)
    {
       try
        {
            var bids = await _bidService.GetBids();
            if(bids == null) return NotFound();
            return Ok(bids);
        }catch(BadHttpRequestException ex)
        {
            _bidLogger.LogWarning("Error {ex}", ex.Message);
            return BadRequest();
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BidResponseDto>>> GetABid([FromRoute]Guid id)
    {
        var bid = await _bidService.GetBid(id);
        return Ok(bid);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteBid([FromRoute] Guid id)
    {
        var userId = "e3b8da6a-3c57-43fa-9c16-723935ca52da";
        if(!Guid.TryParse(userId, out Guid parsed))
        {
            return BadRequest();
        }

        try
        {
            var bid = await _bidService.DeleteBid(id, parsed);
            if(bid == null) return NotFound();
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
        if(!validationResult.IsValid) return BadRequest();
        Console.WriteLine($"{data.ItemId} {data.UserId} {data.Price}");
        try
        {
            var bid = await _bidService.CreateBid(data);
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
        if(!validationResult.IsValid) return BadRequest();

        try
        {
            var bid = await _bidService.ChangeBidStatus(data.Status, id);
            if(bid == null) return NotFound();
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
    public async Task<ActionResult> UpdateABid([FromBody] CreateBidDto data)
    {
        ValidationResult validationResult = await _createBidValidator.ValidateAsync(data);
        if(!validationResult.IsValid) return BadRequest();

        try
        {
            var bid = await _bidService.CreateBid(data);
            return CreatedAtAction(nameof(GetABid), new {id = bid.Id}, bid);
        }catch(BadHttpRequestException ex)
        {
            _bidLogger.LogWarning("Error {ex}", ex.Message);
            return BadRequest();
        }
    }
}