using CrabInABucket.Api.Requests;
using FluentValidation;

namespace CrabInABucket.Api.Validation;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Username)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(20);

        RuleFor(x => x.PasswordConfirmation)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(20)
            .Equal(x => x.Password)
            .WithMessage("Passwords must match.");
    }
}