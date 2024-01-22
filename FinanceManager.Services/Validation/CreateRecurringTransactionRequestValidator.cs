using FinanceManager.Core.Requests;
using FluentValidation;

namespace FinanceManager.Services.Validation;

public class CreateRecurringTransactionRequestValidator : AbstractValidator<CreateRecurringTransactionRequest>
{
    public CreateRecurringTransactionRequestValidator()
    {
        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("Amount cannot be empty or zero");

        RuleFor(x => x.TransactionName)
            .NotEmpty()
            .WithMessage("Please specify name for this recurring transaction.");

        RuleFor(x => x.TransactionInterval)
            .NotEmpty()
            .InclusiveBetween(1, 28)
            .WithMessage("Please specify an interval between 1 an 28 days");
        
        RuleFor(x => x.StartsImmediately)
            .NotNull();

        RuleFor(x => x.RecipientAccountId)
            .NotNull()
            .WithMessage("Please specify a recipient account");
        
        RuleFor(x => x.StartDate)
            .Must((request, startDate) => request.StartsImmediately || startDate != null)
            .WithMessage("Recurring transactions that do not start immediately must have a valid starting date.")
            .Must((_, startDate) => startDate == null || startDate.Value.Date > DateTime.UtcNow.Date)
            .When(request => request.StartsImmediately == false)
            .WithMessage("Start date must be a future date.");

    }
}