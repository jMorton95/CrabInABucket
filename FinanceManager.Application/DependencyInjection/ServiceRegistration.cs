using FinanceManager.Core.Interfaces;
using FinanceManager.Core.Middleware.UserContext;
using FinanceManager.Services.Domain;
using FinanceManager.Services.Generic.Password;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Application.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        
        services.AddScoped<IPasswordUtilities, PasswordUtilities>();
        
        services.AddScoped<IUserTokenService, UserTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserContextService, UserContextService>();
        
        return services;
    }
}