using CrabInABucket.Api.Requests;
using CrabInABucket.Api.Validation;
using FluentValidation;

namespace CrabInABucket.Application.DependencyInjection;

public static class ValidationRegistration
{
    public static IServiceCollection AddEndpointValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
        services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        services.AddScoped<IValidator<GetUserRequest>, GetUserRequestValidator>();

        return services;
    }
}