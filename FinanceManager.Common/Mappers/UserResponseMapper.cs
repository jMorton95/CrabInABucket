using FinanceManager.Common.Entities;

namespace FinanceManager.Common.Mappers;

public record UserProfile(Guid Id, string Username, List<string> Roles, IEnumerable<string> Accounts);

public record NamedUser(Guid Id, string Username);

public static class UserResponseMapper
{
    public static UserProfile ToUserResponse(this User user)
    {
        var userRoles = user.Roles?.Select(x => x.Role!.Name).ToList() ?? [];
        var userAccounts = user.Accounts?.Select(x => x.Name).ToList() ?? [];
        
        return new UserProfile(user.Id, user.Username, userRoles, userAccounts);
    }
    public static NamedUser ToNamedUserResponse(this User user) => new(user.Id, user.Username);
}

