using FinanceManager.Api.RouteHandlers;
using FinanceManager.Common.Entities;
using FinanceManager.Common.Mappers;
using FinanceManager.Common.Responses;
using FinanceManager.Data.Read.Users;
using FinanceManager.Data.Write.Transactions;

namespace FinanceManager.Api.Features.Transactions;

public class Deposit : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/deposit", Handler)
        .WithTags("Deposit an amount to an account")
        .WithRequestValidation<Request>()
        .EnsureEntityExists<Account>(x => x.RecipientAccountId)
        .EnsureEntityExists<User>(x => x.RequesterId)
        .SelfOrAdminResource<User>(x => x.RequesterId);

    private record Request(Guid RequesterId, Guid RecipientAccountId, decimal Amount);

    private record Response(bool Success, string Message) : BasePostResponse(Success, Message);

    private class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.RequesterId)
                .NotEmpty();
        
            RuleFor(x => x.Amount)
                .NotNull()
                .WithMessage("Deposit amount cannot be empty.");

            RuleFor(x => x.RecipientAccountId)
                .NotNull()
                .WithMessage("Please supply an account to deposit to.");
        }
    }

    private static async Task<Results<Ok<Response>, BadRequest<Response>>> Handler (
        Request request,
        IWriteTransaction writeTransaction,
        ITransactionMapper transactionMapper
    )
    {
        var transaction = transactionMapper.BuildTransaction(
            amount: request.Amount,
            recurringTransaction: false,
            recipientAccountId: request.RecipientAccountId,
            senderAccountId: request.RecipientAccountId
        );
        
        var createResult = await writeTransaction.CreateAsync(transaction);
        
        var response = new Response(createResult, createResult ? "Success" : "Error creating transaction.");

        return createResult ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}