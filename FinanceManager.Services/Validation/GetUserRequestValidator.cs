using FinanceManager.Common.Requests;
using FluentValidation;

namespace FinanceManager.Services.Validation;

public class GetUserRequestValidator : AbstractValidator<GetUserRequest>
{
    public GetUserRequestValidator()
    {
        RuleFor(x => x.Username)
            .EmailAddress()
            .NotEmpty();
    }
}