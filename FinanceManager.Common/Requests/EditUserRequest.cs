namespace FinanceManager.Common.Requests;

public record EditUserRequest(Guid Id) : BaseEditRequest(Id);