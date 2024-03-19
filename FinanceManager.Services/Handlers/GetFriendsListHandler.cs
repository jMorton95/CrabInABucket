using FinanceManager.Core.Mappers;
using FinanceManager.Core.Middleware.UserContext;
using FinanceManager.Core.Responses;
using FinanceManager.Data.Read.Friends;

namespace FinanceManager.Services.Handlers;

public interface IGetFriendsListHandler
{
    Task<ResultResponse<FriendListResponse>> HandleGetFriendsList(int numberOfSuggestions);
}

public class GetFriendsListHandler(IReadUserFriends db, IUserContextService userContextService) : IGetFriendsListHandler
{
    private readonly Guid? _currentUserId = userContextService.GetCurrentUserId();
    
    public async Task<ResultResponse<FriendListResponse>> HandleGetFriendsList(int numberOfSuggestions)
    {
        if (_currentUserId is not { } userId)
        {
            return new ResultResponse<FriendListResponse>(false, "Error retrieving friends list.");
        }
        
        var userFriends = db.GetUserFriends(userId);
        var pendingRequests = db.GetPendingFriendRequests(userId);
        var relatedFriends = db.GetRelatedFriends(userId);
        var suggestedFriends = db.GetRandomFriendSuggestions(userId, numberOfSuggestions);

        await Task.WhenAll(userFriends, pendingRequests, relatedFriends, suggestedFriends);

        var result = new FriendListResponse (
            (await userFriends).Select(x => x.ToNamedUserResponse()).ToList(),
            (await pendingRequests).Select(x => x.ToNamedUserResponse()).ToList(),
            (await relatedFriends).Select(x => x.ToNamedUserResponse()).ToList(),
            (await suggestedFriends).Select(x => x.ToNamedUserResponse()).ToList()
        );

        return new ResultResponse<FriendListResponse>(true, "Success", result);
    }
}