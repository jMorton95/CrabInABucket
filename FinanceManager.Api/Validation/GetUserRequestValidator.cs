using FinanceManager.Core.Requests;
using FluentValidation;

namespace FinanceManager.Api.Validation;

public class GetUserRequestValidator : AbstractValidator<GetUserRequest>
{
    public GetUserRequestValidator()
    {
        RuleFor(x => x.Username)
            .EmailAddress()
            .NotEmpty();
    }
}