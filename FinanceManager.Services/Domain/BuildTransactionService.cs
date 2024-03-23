using FinanceManager.Common.DataEntities;

namespace FinanceManager.Services.Domain;

public interface IBuildTransactionService
{
    Transaction MapTransaction (
        decimal amount,
        bool recurringTransaction,
        Guid recipientAccountId,
        Guid? senderAccountId = null
    );
}

public class BuildTransactionService() : IBuildTransactionService
{
    public Transaction MapTransaction(decimal amount, bool recurringTransaction, Guid recipientAccountId, Guid? senderAccountId = null)
    {
        return new Transaction {
            Amount = amount,
            RecurringTransaction = recurringTransaction,
            RecipientAccountId = recipientAccountId,
            SenderAccountId = senderAccountId
        };
    }
}