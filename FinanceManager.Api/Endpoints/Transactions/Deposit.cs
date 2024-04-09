using FinanceManager.Api.RouteHandlers;
using FinanceManager.Common.Contracts;
using FinanceManager.Common.Entities;
using FinanceManager.Common.Mappers;
using FinanceManager.Data.Write.Transactions;

namespace FinanceManager.Api.Endpoints.Transactions;

public class Deposit : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/deposit", Handler)
        .WithTags("Deposit an amount to an account")
        .WithRequestValidation<Request>()
        .EnsureEntityExists<Account>(x => x.RecipientAccountId)
        .EnsureEntityExists<Common.Entities.User>(x => x.RequesterId)
        .SelfOrAdminResource<Common.Entities.User>(x => x.RequesterId);

    public record Request(Guid RequesterId, Guid RecipientAccountId, decimal Amount);

    public record Response(bool Success, string Message): IPostResponse;

    public class RequestValidator : AbstractValidator<Request>
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

    
    private static async Task<Results<Ok<Response>, ValidationError, BadRequest<Response>>> Handler (
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