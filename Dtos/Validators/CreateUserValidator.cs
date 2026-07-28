using bidding_zone_api.Dtos.Request;
using bidding_zone_api.Models;
using FluentValidation;

namespace bidding_zone_api.Dtos.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(prop => prop.Email).NotEmpty().EmailAddress();
        RuleFor(prop => prop.FirstName).NotEmpty().Length(2, 100);
        RuleFor(prop => prop.LastName).NotEmpty().Length(2, 100);
        RuleFor(prop => prop.Password).NotEmpty().Length(6, 100);
        RuleFor(prop => prop.Gender)
            .NotEmpty()
            .IsEnumName(typeof(Gender), caseSensitive: false)
            .WithMessage("Invalid gender value provided");
        When(prop => prop.Address != null, () =>
        {
            RuleFor(prop => prop.Address!).SetValidator(new AddressValidator());
        });
    }
}