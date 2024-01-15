namespace FinanceManager.Core.Requests;

public record EditAccountRequest(Guid Id, string AccountName): BaseEditRequest(Id);