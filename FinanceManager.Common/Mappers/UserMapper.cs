using FinanceManager.Common.Entities;
using FinanceManager.Common.Responses;

namespace FinanceManager.Common.Mappers;

public static class UserMapper
{
    public static UserResponse ToUserResponse(this User user)
    {
        var userRoles = user.Roles?.Select(x => x.Role!.Name).ToList() ?? [];
        var userAccounts = user.Accounts?.Select(x => x.Name).ToList() ?? [];
        
        return new UserResponse(user.Id, user.Username, userRoles, userAccounts);
    }
    public static NamedUserResponse ToNamedUserResponse(this User user) => new(user.Id, user.Username);
}

