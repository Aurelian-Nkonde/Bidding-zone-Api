using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Models;
using FluentValidation;

namespace bidding_zone_api.Dtos.Validators;

public class AddressValidator : AbstractValidator<AddressDto>
{
    public AddressValidator()
    {
        RuleFor(prop => prop.HouseNumber).GreaterThan(0);
        RuleFor(prop => prop.StreetName).NotEmpty().Length(2, 100);
        RuleFor(prop => prop.Surbub).NotEmpty().Length(2, 100);
        RuleFor(prop => prop.Province)
            .NotEmpty()
            .IsEnumName(typeof(Province), caseSensitive: false)
            .WithMessage("Invalid province value provided");;
    }
}