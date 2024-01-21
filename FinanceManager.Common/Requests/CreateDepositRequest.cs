namespace FinanceManager.Core.Requests;

public record CreateDepositRequest(Guid RecipientAccountId, decimal Amount);
