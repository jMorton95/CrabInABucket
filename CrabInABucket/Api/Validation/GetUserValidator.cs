using CrabInABucket.Api.Requests;
using FluentValidation;

namespace CrabInABucket.Api.Validation;

public class GetUserValidator : AbstractValidator<GetUserRequest>
{
    public GetUserValidator()
    {
        RuleFor(x => x.Username)
            .EmailAddress()
            .NotEmpty();
    }
}