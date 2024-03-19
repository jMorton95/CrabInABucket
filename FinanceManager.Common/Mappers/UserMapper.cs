using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Responses;

namespace FinanceManager.Core.Mappers;

public static class UserMapper
{
    public static UserResponses ToUserResponse(this User user)
    {
        var userRoles = user.Roles?.Select(x => x.Role!.Name).ToList() ?? new List<string>();
        var userAccounts = user.Accounts?.Select(x => x.Name).ToList() ?? new List<string>();
        
        return new UserResponses(user.Id, user.Username, userRoles, userAccounts);
    }
    public static NamedUserResponse ToNamedUserResponse(this User user) => new(user.Id, user.Username);
}

