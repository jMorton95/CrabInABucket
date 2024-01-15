using FinanceManager.Core.Requests;
using FinanceManager.Data.Read.Accounts;
using FluentValidation;

namespace FinanceManager.Services.Validation;

public class EditAccountRequestValidator : AbstractValidator<EditAccountRequest>
{
    public EditAccountRequestValidator(IReadAccounts readAccounts)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Please specify an Account to Edit.");

        RuleFor(x => x.AccountName)
            .NotEmpty()
            .WithMessage("Account Name must not be empty and be between 3 and 30 characters.");
        RuleFor(x => x.AccountName)
            .MustAsync(async (name, cancel) => await readAccounts.DoesAccountExist(name) == false)
            .WithMessage("You already have an account with this name.");
        
    }

}