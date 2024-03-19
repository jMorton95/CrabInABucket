using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Responses;
using FinanceManager.Services.Handlers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FinanceManager.Api.Endpoints.Test;

public static class TestEndpoints
{
    public static IEndpointRouteBuilder MapTestEndpoints(this IEndpointRouteBuilder app)
    {
        return app.MapGroup("/api/test").WithTags("test")
            .MapTestGetFriendsEndpoint()
            .MapGetRelatedFriendsEndpoint()
            .MapCheckIfUsersAreFriends()
            .MapGetRandomFriends()
            .MapGetPendingRequests();
    }

    private static IEndpointRouteBuilder MapTestGetFriendsEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/get-friends/{id}", async Task<Results<Ok<List<User>>, BadRequest>> 
            (TestHandlers handler, Guid id) => TypedResults.Ok(await handler.GetFriendships(id)));
            
        return builder;
    }
    
    private static IEndpointRouteBuilder MapGetRelatedFriendsEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/get-related-friends/{id}", async Task<Results<Ok<List<UserResponses>>, BadRequest>> 
            (TestHandlers handler, Guid id) => TypedResults.Ok(await handler.GetRelatedFriends(id)));
            
        return builder;
    }
    
    private static IEndpointRouteBuilder MapCheckIfUsersAreFriends(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/check-friends/{userId}", async Task<Results<Ok<bool>, BadRequest>> 
            (TestHandlers handler, Guid userId) => TypedResults.Ok(await handler.CheckUsersAreFriends(userId)));
            
        return builder;
    }
    
    private static IEndpointRouteBuilder MapGetRandomFriends(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/get-random-friends", async Task<Results<Ok<List<UserResponses>>, BadRequest>> 
            (TestHandlers handler) => TypedResults.Ok(await handler.GetUnrelatedFriends()));
            
        return builder;
    }
    
    private static IEndpointRouteBuilder MapGetPendingRequests(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/get-pending", async Task<Results<Ok<List<UserResponses>>, BadRequest>> 
            (TestHandlers handler) => TypedResults.Ok(await handler.GetPendingRequests()));
            
        return builder;
    }
}