using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Models;
using FluentValidation;

namespace bidding_zone_api.Dtos.Validators;

public class CreateBidValidator : AbstractValidator<CreateBidDto>
{
    public CreateBidValidator()
    {
        RuleFor(prop => prop.ItemId).NotEmpty();
        RuleFor(prop => prop.UserId).NotEmpty();
        RuleFor(prop => prop.Price).NotEmpty();
        // RuleFor(prop => prop.Status)
        //     .NotEmpty()
        //     .IsEnumName(typeof(BidStatus), caseSensitive: false)
        //     .WithMessage("Invalid bid status value provided");
    }
}