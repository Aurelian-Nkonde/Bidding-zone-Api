using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Models;
using FluentValidation;

namespace bidding_zone_api.Dtos.Validators;
public class CreateItemValidator : AbstractValidator<CreateItemDto>
{
    public CreateItemValidator()
    {
        RuleFor(prop => prop.UserId).NotEmpty();
        RuleFor(prop => prop.StartingPrice).NotNull();
        RuleFor(prop => prop.Title).NotEmpty();
        RuleFor(prop => prop.Description).NotEmpty();
        // RuleFor(prop => prop.Image);
        RuleFor(prop => prop.EndTimer)
            .NotEmpty()
            .IsEnumName(typeof(Bidding_times), caseSensitive: false)
            .WithMessage("Invalid endTimer value provided");
    }
}