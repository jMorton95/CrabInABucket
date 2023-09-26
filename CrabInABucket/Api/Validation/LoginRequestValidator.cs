using CrabInABucket.Api.Requests;
using FluentValidation;

namespace CrabInABucket.Api.Validation;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username).EmailAddress().NotEmpty();
        RuleFor(x => x.Password).MinimumLength(8).MaximumLength(20).NotEmpty();
    }
}