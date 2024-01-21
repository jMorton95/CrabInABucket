namespace FinanceManager.Core.Requests;

public record CreateRecurringTransactionRequest (
    decimal Amount,
    int TransactionInterval,
    Guid RecipientAccountId,
    bool StartsImmediately = false,
    Guid? SenderAccountId = null,
    DateTime? StartDate = null
);