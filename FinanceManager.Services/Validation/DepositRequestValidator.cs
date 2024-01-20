using FinanceManager.Core.Requests;
using FluentValidation;

namespace FinanceManager.Services.Validation;

public class DepositRequestValidator : AbstractValidator<DepositRequest>
{
    //TODO: Validate Account exists in database
    public DepositRequestValidator()
    {
        RuleFor(x => x.Amount)
            .NotNull()
            .WithMessage("Deposit amount cannot be empty.");

        RuleFor(x => x.RecipientAccountId)
            .NotNull()
            .WithMessage("Please supply an account to deposit to.");
    }
}