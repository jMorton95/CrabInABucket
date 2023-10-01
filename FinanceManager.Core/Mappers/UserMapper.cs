using FinanceManager.Core.Responses;
using FinanceManager.Core.Models;

namespace FinanceManager.Core.Mappers;

public static class UserMapper
{
    public static UserResponse ToUserResponse(this User user)
    {
        var userRoles = user.Roles?.Select(x => x.Role!.Name).ToList() ?? new List<string>();
        var userAccounts = user.Accounts?.Select(x => x.Name).ToList() ?? new List<string>();
        
        return new UserResponse(user.Username, userRoles, userAccounts);
    }
}