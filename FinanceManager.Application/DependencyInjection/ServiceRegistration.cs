using FinanceManager.Core.Utilities;
using FinanceManager.Services.Generic.Password;
using FinanceManager.Services.Middleware.UserContext;
using FinanceManager.Services.Services;
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
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserContextService, UserContextService>();
        
        

        return services;
    }
}