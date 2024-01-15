namespace FinanceManager.Core.Requests;

public record EditUserRequest(Guid Id) : BaseEditRequest(Id);