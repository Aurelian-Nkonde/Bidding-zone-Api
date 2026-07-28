using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Models;
using FluentValidation;

namespace bidding_zone_api.Dtos.Validators;

public class UpdateBidStatusValidator : AbstractValidator<UpdateBidStatusDto>
{
    public UpdateBidStatusValidator()
    {
        RuleFor(prop => prop.Status)
            .NotEmpty()
            .IsEnumName(typeof(BidStatus), caseSensitive: false)
            .WithMessage("Invalid bid status value provided");
    }
}