using FinanceManager.Api.Features;
using FinanceManager.Api.Features.Auth;
using FinanceManager.Common.RouteHandlers.Filters;

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
            .MapEndpoint<>();

        if (app.Environment.IsDevelopment())
        {
            endpoints.MapGroup("/dev")
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