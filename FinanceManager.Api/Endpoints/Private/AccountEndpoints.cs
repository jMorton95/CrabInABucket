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
        var accountGroup = app.MapGroup("/api/account/")
            .WithTags("Account")
            .RequireAuthorization();
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
            
            return TypedResults.Ok(await handler.CreateAccount(req));
        });
    }
}