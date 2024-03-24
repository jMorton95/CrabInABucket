using FinanceManager.Common.Requests;
using FinanceManager.Common.Responses;
using FinanceManager.Services.Handlers;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Endpoints.Private;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("/api/account/")
            .WithTags("Account")
            .RequireAuthorization()
            .MapEditAccountNameEndpoint();
    }

   

    private static IEndpointRouteBuilder MapEditAccountNameEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/edit", async Task<Results<Ok<BasePostResponse>, ValidationProblem, BadRequest<BasePostResponse>>> (
                [FromBody] EditAccountRequest req,
                IValidator<EditAccountRequest> validator,
                IEditAccountHandler handler
        ) 
            =>
        {
            var validationResult = await validator.ValidateAsync(req);
            
            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await handler.ChangeAccountName(req);

            return result.Success ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
        });
        
        return builder;
    }
}