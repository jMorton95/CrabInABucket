using FinanceManager.Api.RouteHandlers;
using FinanceManager.Common.Entities;
using FinanceManager.Common.Mappers;
using FinanceManager.Common.Responses;
using FinanceManager.Data.Write.Transactions;

namespace FinanceManager.Api.Features.Transactions;

public class Recurring : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/recurring", Handler)
        .WithRequestValidation<Request>()
        .EnsureEntityExists<User>(x => x.RecipientAccountId)
        .SelfOrAdminResource<User>(x => x.SenderAccountId);

    private record Request(
        decimal Amount,
        string TransactionName,
        int TransactionInterval,
        Guid RecipientAccountId,
        Guid SenderAccountId,
        DateTime StartDate,
        bool StartsImmediately
    );

    private record Response(bool Success, string Message) : BasePostResponse(Success, Message);

    private class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty()
                .WithMessage("Amount cannot be empty or zero");

            RuleFor(x => x.TransactionName)
                .NotEmpty()
                .WithMessage("Please specify name for this recurring transaction.");

            RuleFor(x => x.TransactionInterval)
                .NotEmpty()
                .InclusiveBetween(1, 28)
                .WithMessage("Please specify an interval between 1 an 28 days");
        
            RuleFor(x => x.StartsImmediately)
                .NotEmpty();

            RuleFor(x => x.RecipientAccountId)
                .NotNull()
                .WithMessage("Please specify a recipient account");

            RuleFor(x => x.SenderAccountId)
                .NotEmpty()
                .WithMessage("Error occured accessing your User Id");
        
            RuleFor(x => x.StartDate)
                .Must((request, startDate) => request.StartsImmediately || startDate != null)
                .WithMessage("Recurring transactions that do not start immediately must have a valid starting date.")
                .Must((_, startDate) => startDate == null || startDate.Date > DateTime.UtcNow.Date)
                .When(request => request.StartsImmediately == false)
                .WithMessage("Start date must be a future date.");
        }
    }

    private static async Task<Results<Ok<Response>, BadRequest<Response>>> Handler(
        Request request,
        IWriteTransaction writeTransaction,
        IWriteRecurringTransaction writeRecurringTransaction,
        ITransactionMapper transactionMapper)
    {
        var recurringTransaction = request.StartsImmediately
            ? transactionMapper
                .BuildImmediateTransaction(request.Amount,
                    request.TransactionName,
                    request.TransactionInterval,
                    request.RecipientAccountId,
                    request.SenderAccountId
                )
            : transactionMapper
                .BuildDelayedTransaction(
                    request.Amount, 
                    request.TransactionName, 
                    request.TransactionInterval, 
                    request.RecipientAccountId, 
                    request.SenderAccountId, 
                    request.StartDate
                );

        
        var createResult = await writeRecurringTransaction.CreateAsync(recurringTransaction);

        if (!createResult)
        {
            return TypedResults.BadRequest(new Response(false, "Error occurred creating recurring transaction"));
        }

        if (!request.StartsImmediately)
        {
            return TypedResults.Ok(new Response(true, "Transaction created successfully."));
        }
        
        var transaction = transactionMapper.BuildTransaction(
            request.Amount,
            recurringTransaction: true,
            request.RecipientAccountId,
            request.SenderAccountId
        );

        var transactionResult = await writeTransaction.CreateAsync(transaction);

        var response = new Response(transactionResult, transactionResult
            ? $"Successfully created recurring transaction and deposited {request.Amount}"
            : "Successfully created recurring transaction.");

        return transactionResult ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}