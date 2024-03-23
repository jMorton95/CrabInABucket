namespace FinanceManager.Common.Requests;

public record CreateRecurringTransactionRequest (
    decimal Amount,
    string TransactionName,
    int TransactionInterval,
    Guid RecipientAccountId,
    bool StartsImmediately = false,
    Guid? SenderAccountId = null,
    DateTime? StartDate = null
);