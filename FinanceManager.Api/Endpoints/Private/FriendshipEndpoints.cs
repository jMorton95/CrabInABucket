using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Services.Handlers;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Endpoints.Private;

public static class FriendshipEndpoints
{
    public static void MapFriendshipEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("api/friendship/")
            .WithTags("friendships")
            .RequireAuthorization()
            .MapFriendRequestEndpoint();
    }

    private static IEndpointRouteBuilder MapFriendRequestEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/request", async Task<Results<Ok<BasePostResponse>, ValidationProblem, BadRequest<BasePostResponse>>> (
                [FromBody] CreateFriendshipRequest req,
                IValidator<CreateFriendshipRequest> validator,
                ICreateFriendshipHandler handler
        ) 
            =>
        {
            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await handler.CreateFriendship(req);

            return result.Success ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
        }).WithName("request");
        
        return builder;
    }
}