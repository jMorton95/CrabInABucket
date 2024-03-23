namespace FinanceManager.Common.Requests;

public record CreateDepositRequest(Guid RecipientAccountId, decimal Amount);
