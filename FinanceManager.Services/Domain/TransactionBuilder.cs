using FinanceManager.Common.DataEntities;

namespace FinanceManager.Services.Domain;

public interface ITransactionBuilder
{
    Transaction Build (
        decimal amount,
        bool recurringTransaction,
        Guid recipientAccountId,
        Guid? senderAccountId = null
    );
}

public class TransactionBuilder() : ITransactionBuilder
{
    public Transaction Build(decimal amount, bool recurringTransaction, Guid recipientAccountId, Guid? senderAccountId = null)
    {
        return new Transaction {
            Amount = amount,
            RecurringTransaction = recurringTransaction,
            RecipientAccountId = recipientAccountId,
            SenderAccountId = senderAccountId
        };
    }
}