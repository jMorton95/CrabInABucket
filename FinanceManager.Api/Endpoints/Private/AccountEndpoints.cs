using System.ComponentModel.DataAnnotations;
using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Core.Validation;
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
            .MapCreateAccountEndpoint();
    }

    private static void MapCreateAccountEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/create", async Task<Results<Ok<BasePostResponse>, ValidationProblem, BadRequest>> (
            [FromBody] CreateAccountRequest req,
            IValidator<CreateAccountRequest> validator,
            ICreateAccountHandler handler
        ) =>
        {
            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await handler.CreateAccount(req);
            
            
            return result.Success ? TypedResults.Ok(result) : TypedResults.BadRequest();
        }).WithName("create");
    }

    private static void MapEditAccountNameEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/edit", async Task<Results<Ok<BasePostResponse>, ValidationProblem, BadRequest>> (
                [FromBody] EditAccountRequest req,
                IValidator<EditAccountRequest> validator,
                IEditAccountHandler handler)
            =>
        {
            var validationResult = await validator.ValidateAsync(req);
            
            
            
            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await handler.ChangeAccountName(req);

            return result.Success ? TypedResults.Ok(result) : TypedResults.BadRequest();
        });
    }
}