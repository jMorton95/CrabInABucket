using FinanceManager.Core.Requests;
using FinanceManager.Services.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Application.DependencyInjection;

public static class HandlerRegistration
{
    public static void AddServiceHandlers(this IServiceCollection services)
    {
        services.AddScoped<ILoginHandler, LoginHandler>();
        services.AddScoped<ICreateUserHandler, CreateUserHandler>();
        services.AddScoped<IRoleHandler, RoleHandler>();
        services.AddScoped<ICreateAccountHandler, CreateAccountHandler>();
        services.AddScoped<IEditAccountHandler, EditAccountHandler>();
        services.AddScoped<ICreateDepositHandler, CreateDepositHandler>();
        services.AddScoped<ICreateFriendshipHandler, CreateFriendshipHandler>();
        services.AddScoped<IFriendshipRequestStatusHandler, FriendshipRequestStatusHandler>();
        services.AddScoped<IGetFriendsListHandler, GetFriendsListHandler>();
        services.AddScoped<TestHandlers>();
    }
}