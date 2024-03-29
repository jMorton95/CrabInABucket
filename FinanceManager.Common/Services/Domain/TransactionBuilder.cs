using FinanceManager.Common.DataEntities;

namespace FinanceManager.Services.Domain;

public interface ITransactionMapper
{
    Transaction BuildTransaction (
        decimal amount,
        bool recurringTransaction,
        Guid recipientAccountId,
        Guid? senderAccountId = null
    );

    RecurringTransaction BuildImmediateTransaction(
        decimal Amount,
        string transactionName,
        int transactionInterval,
        Guid recipientAccountId,
        Guid senderAccountId
    );

    RecurringTransaction BuildDelayedTransaction(decimal amount,
        string transactionName,
        int transactionInterval,
        Guid recipientAccountId,
        Guid senderAccountId,
        DateTime startDate
    );
}

public class TransactionMapper : ITransactionMapper
{
    public Transaction BuildTransaction(decimal amount, bool recurringTransaction, Guid recipientAccountId, Guid? senderAccountId = null)
    {
        return new Transaction {
            Amount = amount,
            RecurringTransaction = recurringTransaction,
            RecipientAccountId = recipientAccountId,
            SenderAccountId = senderAccountId
        };
    }
    
    public RecurringTransaction BuildImmediateTransaction(decimal amount, string transactionName, int transactionInterval, Guid recipientAccountId, Guid senderAccountId) 
    {
        return new RecurringTransaction
        {
            Amount = amount,
            TransactionName = transactionName,
            TransactionInterval = transactionInterval,
            LastTransactionDate = DateTime.UtcNow,
            NextTransactionDate = DateTime.UtcNow.AddDays(transactionInterval),
            RecipientAccountId = recipientAccountId,
            SenderAccountId = senderAccountId,
        };
    }

    public RecurringTransaction BuildDelayedTransaction(decimal amount, string transactionName, int transactionInterval, Guid recipientAccountId, Guid senderAccountId, DateTime startDate)
    {
        return new RecurringTransaction
        {
            Amount = amount,
            TransactionName = transactionName,
            TransactionInterval = transactionInterval,
            LastTransactionDate = null,
            NextTransactionDate = startDate,
            RecipientAccountId = recipientAccountId,
            SenderAccountId = senderAccountId
        };
    }
}