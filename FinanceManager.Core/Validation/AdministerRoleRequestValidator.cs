using FinanceManager.Core.Requests;
using FluentValidation;
using Microsoft.Win32;

namespace FinanceManager.Core.Validation;

public class AdministerRoleRequestValidator : AbstractValidator<AdministerRoleRequest>
{
    public AdministerRoleRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.IsAdmin)
            .NotEmpty();
    }
}