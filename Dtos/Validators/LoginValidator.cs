using bidding_zone_api.Dtos.Request;
using FluentValidation;

namespace bidding_zone_api.Dtos.Validators;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(prop => prop.Email).NotEmpty().EmailAddress();
        RuleFor(prop => prop.Password).NotEmpty().Length(6, 100);
    }
}