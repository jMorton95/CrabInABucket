
namespace FinanceManager.Core.Responses;

public record UserResponse(string Username, List<string> Roles, IEnumerable<string> Accounts);