namespace FinanceManager.Common.Requests;

public record CreateDepositRequest(Guid RequesterId, Guid RecipientAccountId, decimal Amount);
