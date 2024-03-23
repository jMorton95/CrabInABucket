using FinanceManager.Common.Requests;
using FinanceManager.Data.Read;
using FinanceManager.Data.Read.Friendships;
using FluentValidation;

namespace FinanceManager.Services.Validation;

public class CreateFriendshipRequestValidator : AbstractValidator<CreateFriendshipRequest>
{
    public CreateFriendshipRequestValidator(IReadFriendships readFriendships)
    {
        RuleFor(x => x.TargetUserId)
            .NotEmpty()
            .WithMessage("You must provide a valid person to request friendship.")
            .MustAsync(async (x, token) => await readFriendships.DoesFriendshipExist(x) != true)
            .WithMessage("You've already requested to be friends with that person.");
    }
}