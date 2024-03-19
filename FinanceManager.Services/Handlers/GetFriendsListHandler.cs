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
        
        var userFriends = await db.GetUserFriends(userId);
        var pendingRequests = await db.GetPendingFriendRequests(userId);
        var suggestedFriends = await db.GetRelatedFriends(userId);
        var randomFriends = await db.GetRandomFriendSuggestions(userId, numberOfSuggestions);
        
        var result = new FriendListResponse (
            userFriends.Select(x => x.ToNamedUserResponse()).ToList(),
            pendingRequests.Select(x => x.ToNamedUserResponse()).ToList(),
            suggestedFriends.Select(x => x.ToNamedUserResponse()).ToList(),
            randomFriends.Select(x => x.ToNamedUserResponse()).ToList()
        );

        return new ResultResponse<FriendListResponse>(true, "Success", result);
    }
}