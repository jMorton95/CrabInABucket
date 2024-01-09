using FinanceManager.Core.Requests;
using FluentValidation;

namespace FinanceManager.Core.Validation;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username).EmailAddress().NotEmpty();
        RuleFor(x => x.Password).MinimumLength(8).MaximumLength(20).NotEmpty();
    }
}