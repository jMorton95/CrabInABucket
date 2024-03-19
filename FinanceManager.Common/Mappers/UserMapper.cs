using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Responses;

namespace FinanceManager.Core.Mappers;

public static class UserMapper
{
    public static UserResponse ToUserResponse(this User user)
    {
        var userRoles = user.Roles?.Select(x => x.Role!.Name).ToList() ?? new List<string>();
        var userAccounts = user.Accounts?.Select(x => x.Name).ToList() ?? new List<string>();
        
        return new UserResponse(user.Id, user.Username, userRoles, userAccounts);
    }
}