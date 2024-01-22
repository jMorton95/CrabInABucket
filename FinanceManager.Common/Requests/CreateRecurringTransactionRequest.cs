namespace FinanceManager.Core.Requests;

public record CreateRecurringTransactionRequest (
    decimal Amount,
    string TransactionName,
    int TransactionInterval,
    Guid RecipientAccountId,
    bool StartsImmediately = false,
    Guid? SenderAccountId = null,
    DateTime? StartDate = null
);