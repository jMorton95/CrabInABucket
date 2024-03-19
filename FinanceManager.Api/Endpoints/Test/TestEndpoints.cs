using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Responses;
using FinanceManager.Services.Handlers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FinanceManager.Api.Endpoints.Test;

public static class TestEndpoints
{
    public static IEndpointRouteBuilder MapTestEndpoints(this IEndpointRouteBuilder app)
    {
        return app.MapGroup("/api/test").WithTags("test").MapTestGetFriendsEndpoint().MapGetRelatedFriendsEndpoint();
    }

    public static IEndpointRouteBuilder MapTestGetFriendsEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/get-friends/{id}", async Task<Results<Ok<List<User>>, BadRequest>> 
            (TestHandlers handler, Guid id) => TypedResults.Ok(await handler.GetFriendships(id)));
            
        return builder;
    }
    
    public static IEndpointRouteBuilder MapGetRelatedFriendsEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/get-related-friends/{id}", async Task<Results<Ok<List<UserResponse>>, BadRequest>> 
            (TestHandlers handler, Guid id) => TypedResults.Ok(await handler.GetRelatedFriends(id)));
            
        return builder;
    }
}