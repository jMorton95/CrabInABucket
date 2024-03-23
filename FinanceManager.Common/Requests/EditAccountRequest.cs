namespace FinanceManager.Common.Requests;

public record EditAccountRequest(Guid Id, string AccountName): BaseEditRequest(Id);