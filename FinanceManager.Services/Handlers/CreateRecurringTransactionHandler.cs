using System.Diagnostics;
using FinanceManager.Common.DataEntities;
using FinanceManager.Common.Requests;
using FinanceManager.Common.Responses;
using FinanceManager.Data.Write.Transactions;
using FinanceManager.Services.Domain;

namespace FinanceManager.Services.Handlers;

public interface ICreateRecurringTransactionHandler
{
    Task<BasePostResponse> Create(CreateRecurringTransactionRequest request);
}

public class CreateRecurringTransactionHandler
(
    IWriteTransaction writeTransaction,
    IWriteRecurringTransaction writeRecurringTransaction,
    IBuildTransactionService buildTransactionService
)
    : ICreateRecurringTransactionHandler
{
    public async Task<BasePostResponse> Create(CreateRecurringTransactionRequest request)
    {
        var recurringTransaction = request.StartsImmediately
            ? BuildImmediateTransaction(request)
            : BuildDelayedTransaction(request);

       if (recurringTransaction == null)
       {
           //This case shouldn't be hit. Validation logic should fail this edge case before entering the handler.
           return new BasePostResponse(false, "Exceptional Error occured. Please ensure a valid Start Date is selected");
       }
       
        var createResult = await writeRecurringTransaction.CreateAsync(recurringTransaction);

        if (!createResult)
        {
            return new BasePostResponse(false, "Error occurred creating recurring transaction");
        }

        if (!request.StartsImmediately)
        {
            return new BasePostResponse(true, "Transaction created successfully.");
        }
        
        var transaction = buildTransactionService.MapTransaction(
            request.Amount,
            recurringTransaction: true,
            request.RecipientAccountId,
            request.SenderAccountId
        );

        var transactionResult = await writeTransaction.CreateAsync(transaction);

        return new BasePostResponse(transactionResult, transactionResult
            ? $"Successfully created recurring transaction and deposited {request.Amount}"
            : "Successfully created recurring transaction.");
    }

    private RecurringTransaction BuildImmediateTransaction(CreateRecurringTransactionRequest req)
    {
        return new RecurringTransaction
        {
            Amount = req.Amount,
            TransactionName = req.TransactionName,
            TransactionInterval = req.TransactionInterval,
            LastTransactionDate = DateTime.UtcNow,
            NextTransactionDate = DateTime.UtcNow.AddDays(req.TransactionInterval),
            RecipientAccountId = req.RecipientAccountId,
            SenderAccountId = req.SenderAccountId,
        };
    }

    private RecurringTransaction? BuildDelayedTransaction(CreateRecurringTransactionRequest req)
    {
        if (req.StartDate == null)
        {
            //Validation Logic should never allow this case to occur.
            //This is truly exceptional behaviour.
            return null;
        }

        return new RecurringTransaction
        {
            Amount = req.Amount,
            TransactionName = req.TransactionName,
            LastTransactionDate = null,
            NextTransactionDate = req.StartDate.Value,
            RecipientAccountId = req.RecipientAccountId,
            SenderAccountId = req.SenderAccountId
        };
    }
}