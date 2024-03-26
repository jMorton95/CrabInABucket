using FinanceManager.Common.DataEntities;
using FinanceManager.Common.Responses;
using FinanceManager.Common.RouteHandlers;
using FinanceManager.Data.Read.Users;
using FinanceManager.Data.Write.Transactions;
using FinanceManager.Services.Domain;

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
        public RequestValidator(IReadUsers readUsers)
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
        ITransactionBuilder transactionBuilder,
        IWriteTransaction writeTransaction
    )
    {
        var transaction = transactionBuilder.Build(
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