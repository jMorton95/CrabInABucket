using FinanceManager.Common.Requests;
using FinanceManager.Common.Responses;
using FinanceManager.Services.Handlers;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Endpoints.Private;

public static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("api/transaction/")
            .WithTags("account")
            .RequireAuthorization()
            .MapDepositEndpoint();
    }

    private static IEndpointRouteBuilder MapDepositEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/deposit", async Task<Results<Ok<BasePostResponse>, ValidationProblem, BadRequest<BasePostResponse>>> (
            [FromBody] CreateDepositRequest req,
            IValidator<CreateDepositRequest> validator,
            ICreateDepositHandler handler
        )
            =>
        {
            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            var handlerResult = await handler.Deposit(req);

            return handlerResult.Success ? TypedResults.Ok(handlerResult) : TypedResults.BadRequest(handlerResult);

        }).WithName("deposit");

        return builder;
    }

    private static IEndpointRouteBuilder MapRecurringTransactionEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/recurring", async Task<Results<Ok<BasePostResponse>, ValidationProblem, BadRequest<BasePostResponse>>> (
            [FromBody] CreateRecurringTransactionRequest req,
            IValidator<CreateRecurringTransactionRequest> validator,
            ICreateRecurringTransactionHandler handler
        )
            =>
        {
            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            var handlerResult = await handler.Create(req);

            return handlerResult.Success ? TypedResults.Ok(handlerResult) : TypedResults.BadRequest(handlerResult);
            
        }).WithName("recurring");

        return builder;
    }
}