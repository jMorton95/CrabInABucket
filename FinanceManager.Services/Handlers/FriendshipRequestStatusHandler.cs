using FinanceManager.Core.Middleware.UserContext;
using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Data.Write.Friendships;

namespace FinanceManager.Services.Handlers;

public interface IFriendshipRequestStatusHandler
{
    Task<BasePostResponse> UpdateFriendshipStatus(FriendshipRequestStatusUpdateRequest req);
}

public class FriendshipRequestStatusHandler(IWriteFriendships writeFriendships) : IFriendshipRequestStatusHandler
{
    public async Task<BasePostResponse> UpdateFriendshipStatus(FriendshipRequestStatusUpdateRequest req)
    {
        var result = await writeFriendships.EditAsync(req);

        return new BasePostResponse(result, result ? "Success" : "Error confirming friendship decision");
    }
}