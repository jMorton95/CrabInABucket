using FinanceManager.Core.Requests;
using FinanceManager.Data.Read.Accounts;
using FluentValidation;

namespace FinanceManager.Services.Validation;

public class CreateDepositRequestValidator : AbstractValidator<CreateDepositRequest>
{
    //TODO: Validate Account exists in database
    public CreateDepositRequestValidator(IReadAccounts readAccounts)
    {
        RuleFor(x => x.Amount)
            .NotNull()
            .WithMessage("Deposit amount cannot be empty.");

        RuleFor(x => x.RecipientAccountId)
            .NotNull()
            .WithMessage("Please supply an account to deposit to.")
            .MustAsync(async (x, token) => await readAccounts.GetByIdAsync(x) != null)
            .WithMessage("The account you tried to deposit to does not exist.");
    }
}