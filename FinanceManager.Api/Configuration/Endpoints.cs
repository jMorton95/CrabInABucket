using FinanceManager.Api.Endpoints;
using FinanceManager.Api.Endpoints.Accounts;
using FinanceManager.Api.Endpoints.Auth;
using FinanceManager.Api.Endpoints.Friendships;
using FinanceManager.Api.Endpoints.Transactions;
using FinanceManager.Api.Endpoints.Users;
using FinanceManager.Api.RouteHandlers.Filters;

namespace FinanceManager.Api.Configuration;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("api")
            .RequireAuthorization()
            .AddEndpointFilter<RequestLoggingFilter>();

        endpoints.MapGroup("/auth")
            .WithTags("Authentication")
            .AllowAnonymous()
            .MapEndpoint<Login>()
            .MapEndpoint<Register>();

        endpoints.MapGroup("/account")
            .WithTags("Account")
            .MapEndpoint<Create>()
            .MapEndpoint<Edit>();

        endpoints.MapGroup("/friendships")
            .WithTags("Friendships")
            .MapEndpoint<RequestFriendship>()
            .MapEndpoint<RespondToFriendship>()
            .MapEndpoint<FriendsList>();

        endpoints.MapGroup("/transaction")
            .WithTags("Transactions")
            .MapEndpoint<Deposit>()
            .MapEndpoint<Recurring>();

        endpoints.MapGroup("/users")
            .WithTags("Users")
            .MapEndpoint<GetAll>()
            .MapEndpoint<GetByEmail>()
            .MapEndpoint<ChangeAdminRole>();

        if (app.Environment.IsDevelopment())
        {
            endpoints.MapGroup("/test")
                .AllowAnonymous()
                .WithTags("Development");
        }
    }
    
    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}