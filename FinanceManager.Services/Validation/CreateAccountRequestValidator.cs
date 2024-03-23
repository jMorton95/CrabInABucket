using FinanceManager.Common.Requests;
using FluentValidation;

namespace FinanceManager.Services.Validation;

public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
    {
        RuleFor(x => x.AccountName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(30)
            .WithMessage("Account Name must not be empty and be between 3 and 30 characters.");
    }
}