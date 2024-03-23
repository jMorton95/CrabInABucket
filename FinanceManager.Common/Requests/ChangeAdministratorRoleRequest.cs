namespace FinanceManager.Common.Requests;

public record ChangeAdministratorRoleRequest(Guid UserId, bool IsAdmin);