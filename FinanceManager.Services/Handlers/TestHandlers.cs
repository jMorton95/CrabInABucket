using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Mappers;
using FinanceManager.Core.Middleware.UserContext;
using FinanceManager.Core.Responses;
using FinanceManager.Data.Read;
using FinanceManager.Data.Read.Friends;
using FinanceManager.Data.Read.Users;

namespace FinanceManager.Services.Handlers;

public class TestHandlers(IReadUserFriends friendships, IUserContextService userContextService)
{
    private readonly Guid? _currentUser = userContextService.GetCurrentUserId();
    public async Task<List<User>> GetFriendships(Guid userId)
    {
        return await friendships.GetUserFriends(userId);
    }

    public async Task<List<UserResponse>> GetRelatedFriends(Guid userId)
    {
        var result = await friendships.GetRelatedFriends(userId);

        return result.Select(x => x.ToUserResponse()).ToList();
    }

    public async Task<bool> CheckUsersAreFriends(Guid userId)
    {
        if (_currentUser is null) return false;
        
        return await friendships.CheckUsersAreFriends(_currentUser.Value, userId);
    }
}