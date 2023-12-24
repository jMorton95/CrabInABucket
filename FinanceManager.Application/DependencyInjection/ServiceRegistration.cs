using FinanceManager.Core.Utilities;
using FinanceManager.Services.Services;
using FinanceManager.Services.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Application.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserContextService, UserContextService>();

        return services;
    }
}