using FinanceManager.Core.Interfaces;
using FinanceManager.Core.Middleware.UserContext;
using FinanceManager.Core.Utilities;
using FinanceManager.Services.Generic.Password;
using FinanceManager.Services.Services;
using FinanceManager.Services.Services.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Application.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        
        services.AddScoped<IPasswordUtilities, PasswordUtilities>();
        
        services.AddScoped<IUserTokenService, UserTokenService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserContextService, UserContextService>();
        
        return services;
    }
}