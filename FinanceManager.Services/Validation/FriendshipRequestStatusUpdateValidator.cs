using FinanceManager.Core.Requests;
using FluentValidation;

namespace FinanceManager.Services.Validation;

public class FriendshipRequestStatusUpdateValidator : AbstractValidator<FriendshipRequestStatusUpdateRequest>
{
    public FriendshipRequestStatusUpdateValidator()
    {
        RuleFor(x => x.Accepted)
            .NotNull()
            .WithMessage("Error occurred parsing your decision.");

        RuleFor(x => x.FriendshipId)
            .NotEmpty()
            .WithMessage("Error occurred processing Friendship Request");
    }
}