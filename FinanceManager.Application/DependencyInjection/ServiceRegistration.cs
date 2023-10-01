using FinanceManager.Api.Core.Services;
using FinanceManager.Api.Core.Services.Interfaces;

namespace FinanceManager.Application.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IUserAccessorService, UserAccessorService>();

        return services;
    }
}