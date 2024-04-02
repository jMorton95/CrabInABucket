
namespace FinanceManager.Common.Responses;

public record UserResponse(Guid Id, string Username, List<string> Roles, IEnumerable<string> Accounts);

public record NamedUserResponse(Guid Id, string Username);
