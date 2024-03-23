using FinanceManager.Common.Requests;
using FluentValidation;

namespace FinanceManager.Services.Validation;

public class AdministerRoleRequestValidator : AbstractValidator<ChangeAdministratorRoleRequest>
{
    public AdministerRoleRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.IsAdmin)
            .NotEmpty();
    }
}