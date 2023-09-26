using CrabInABucket.Api.Requests;
using CrabInABucket.Api.Validation;
using FluentValidation;

namespace CrabInABucket.Core.Configurators;
public static class ValidationConfiguration
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
        services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        services.AddScoped<IValidator<GetUserRequest>, GetUserRequestValidator>();
    }
}