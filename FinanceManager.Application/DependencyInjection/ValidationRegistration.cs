using FinanceManager.Core.Requests;
using FinanceManager.Core.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Application.DependencyInjection;

public static class ValidationRegistration
{
    public static IServiceCollection AddEndpointValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
        services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        services.AddScoped<IValidator<GetUserRequest>, GetUserRequestValidator>();
        services.AddScoped<IValidator<AdministerRoleRequest>, AdministerRoleRequestValidator>();

        return services;
    }
}