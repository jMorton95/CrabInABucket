using FinanceManager.Core.Requests;
using FinanceManager.Services.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Application.DependencyInjection;

public static class ValidationRegistration
{
    public static IServiceCollection AddRequestValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<GetUserRequest>, GetUserRequestValidator>();
        services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
        services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        services.AddScoped<IValidator<ChangeAdministratorRoleRequest>, AdministerRoleRequestValidator>();
        services.AddScoped<IValidator<CreateAccountRequest>, CreateAccountRequestValidator>();
        services.AddScoped<IValidator<EditAccountRequest>, EditAccountRequestValidator>();
        services.AddScoped<IValidator<CreateDepositRequest>, CreateDepositRequestValidator>();
        services.AddScoped<IValidator<CreateRecurringTransactionRequest>, CreateRecurringTransactionRequestValidator>();
        services.AddScoped<IValidator<CreateFriendshipRequest>, CreateFriendshipRequestValidator>();
        
        return services;
    }
}