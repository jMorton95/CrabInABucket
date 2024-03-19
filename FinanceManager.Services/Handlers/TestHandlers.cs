using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Mappers;
using FinanceManager.Core.Responses;
using FinanceManager.Data.Read;
using FinanceManager.Data.Read.Friends;
using FinanceManager.Data.Read.Users;

namespace FinanceManager.Services.Handlers;

public class TestHandlers(IReadUserFriends friendships)
{
    public async Task<List<User>> GetFriendships(Guid userId)
    {
        return await friendships.GetUserFriends(userId);
    }

    public async Task<List<UserResponse>> GetRelatedFriends(Guid userId)
    {
        var result = await friendships.GetRelatedFriends(userId);

        return result.Select(x => x.ToUserResponse()).ToList();
    }
}