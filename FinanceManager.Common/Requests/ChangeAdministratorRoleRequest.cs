namespace FinanceManager.Core.Requests;

public record ChangeAdministratorRoleRequest(Guid UserId, bool IsAdmin);