namespace FinanceManager.Core.Requests;

public record AdministerRoleRequest(Guid UserId, bool IsAdmin);