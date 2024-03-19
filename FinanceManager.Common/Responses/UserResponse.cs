
namespace FinanceManager.Core.Responses;

public record UserResponse(Guid Id, string Username, List<string> Roles, IEnumerable<string> Accounts);