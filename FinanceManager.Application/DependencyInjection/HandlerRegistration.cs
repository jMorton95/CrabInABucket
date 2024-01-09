using FinanceManager.Services.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Application.DependencyInjection;

public static class HandlerRegistration
{
    public static IServiceCollection AddServiceHandlers(this IServiceCollection services)
    {
        services.AddScoped<ILoginHandler, LoginHandler>();
        services.AddScoped<ICreateUserHandler, CreateUserHandler>();
        services.AddScoped<IRoleHandler, RoleHandler>();
        services.AddScoped<ICreateAccountHandler, CreateAccountHandler>();

        return services;
    }
}