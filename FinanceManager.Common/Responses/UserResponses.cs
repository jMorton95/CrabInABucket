
namespace FinanceManager.Common.Responses;

public record UserResponses(Guid Id, string Username, List<string> Roles, IEnumerable<string> Accounts);

public record NamedUserResponse(Guid Id, string Username);
