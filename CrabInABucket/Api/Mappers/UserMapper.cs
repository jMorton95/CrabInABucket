using CrabInABucket.Api.Responses;
using CrabInABucket.Data.Models;

namespace CrabInABucket.Api.Mappers;

public static class UserMapper
{
    public static UserResponse ToUserResponse(this User user)
    {
        return new UserResponse(user.Username, user.Roles ?? new List<UserRole>(), user.Accounts ?? new List<Account>());
    }
}