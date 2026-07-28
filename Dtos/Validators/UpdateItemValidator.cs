using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Models;
using FluentValidation;

namespace bidding_zone_api.Dtos.Validators;

public class UpdateItemValidator : AbstractValidator<UpdateItemDto>
{
    public UpdateItemValidator()
    {
        RuleFor(prop => prop.Title).NotEmpty().Length(2, 100);
        RuleFor(prop => prop.Description).NotEmpty().Length(10, 100);
        RuleFor(prop => prop.StartingPrice);
        RuleFor(prop => prop.EndTimer)
            .NotEmpty()
            .IsEnumName(typeof(Bidding_times), caseSensitive: false);
    }
}