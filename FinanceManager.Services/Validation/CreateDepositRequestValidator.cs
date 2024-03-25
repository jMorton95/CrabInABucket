using FinanceManager.Common.Requests;
using FinanceManager.Data.Read.Accounts;
using FinanceManager.Data.Read.Users;
using FluentValidation;

namespace FinanceManager.Services.Validation;

public class CreateDepositRequestValidator : AbstractValidator<CreateDepositRequest>
{
    //TODO: Validate Account exists in database
    public CreateDepositRequestValidator(IReadAccounts readAccounts, IReadUsers readUsers)
    {
        RuleFor(x => x.RequesterId)
            .NotEmpty()
            .MustAsync(async (x, _) => await readUsers.GetByIdAsync(x) != null);
        
        RuleFor(x => x.Amount)
            .NotNull()
            .WithMessage("Deposit amount cannot be empty.");

        RuleFor(x => x.RecipientAccountId)
            .NotNull()
            .WithMessage("Please supply an account to deposit to.")
            .MustAsync(async (request, x, _) => await readAccounts.GetOwnedEntityByIdAsync(request.RequesterId, x) != null)
            .WithMessage("The account you tried to deposit to does not exist.");
    }
}