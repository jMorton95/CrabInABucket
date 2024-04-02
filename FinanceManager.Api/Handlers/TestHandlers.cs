using FinanceManager.Common.Entities;
using FinanceManager.Common.Mappers;
using FinanceManager.Common.Responses;
using FinanceManager.Common.Services;
using FinanceManager.Data.Read.Friends;

namespace FinanceManager.Api.Handlers;

public class TestHandlers(IReadUserFriends friendships, IUserContextService userContextService)
{
    private readonly Guid? _currentUser = userContextService.GetCurrentUserId();
    public async Task<List<User>> GetFriendships(Guid userId)
    {
        return await friendships.GetUserFriends(userId);
    }

    public async Task<List<UserResponses>> GetRelatedFriends(Guid userId)
    {
        var result = await friendships.GetRelatedFriends(userId);

        return result.Select(x => x.ToUserResponse()).ToList();
    }

    public async Task<bool> CheckUsersAreFriends(Guid userId)
    {
        if (_currentUser is null) return false;
        
        return await friendships.CheckUsersAreFriends(_currentUser.Value, userId);
    }

    public async Task<List<UserResponses>> GetUnrelatedFriends()
    {
        if (_currentUser is null) return new List<UserResponses>();
        
        var result = await friendships.GetRandomFriendSuggestions(_currentUser.Value, 4);

        return result.Select(x => x.ToUserResponse()).ToList();
    }

    public async Task<List<UserResponses>> GetPendingRequests()
    {
        if (_currentUser is null) return new List<UserResponses>();
        
        var result = await friendships.GetPendingFriendRequests(_currentUser.Value);
        
        return result.Select(x => x.ToUserResponse()).ToList();
    }
}