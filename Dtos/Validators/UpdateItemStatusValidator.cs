using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Models;
using FluentValidation;

namespace bidding_zone_api.Dtos.Validators;

public class UpdateItemStatusValidator : AbstractValidator<UpdateItemStatusDtos>
{
    public UpdateItemStatusValidator()
    {
        RuleFor(prop => prop.Status)
            .NotNull()
            .IsEnumName(typeof(ItemStatus), caseSensitive: false);
    }
}