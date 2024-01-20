namespace FinanceManager.Core.Requests;

public record DepositRequest(Guid RecipientAccountId, decimal Amount, bool RecurringTransaction = false);
