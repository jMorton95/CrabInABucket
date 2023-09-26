using CrabInABucket.Api.Requests;
using FluentValidation;

namespace CrabInABucket.Api.Validation;

public class GetUserRequestValidator : AbstractValidator<GetUserRequest>
{
    public GetUserRequestValidator()
    {
        RuleFor(x => x.Username)
            .EmailAddress()
            .NotEmpty();
    }
}