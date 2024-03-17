using FinanceManager.Core.Middleware.UserContext;
using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Data.Write.Friendships;

namespace FinanceManager.Services.Handlers;

public interface ICreateFriendshipHandler
{
    Task<BasePostResponse> CreateFriendship(CreateFriendshipRequest req);
}

public class CreateFriendshipHandler(IUserContextService userContextService, IWriteFriendships write) : ICreateFriendshipHandler
{
    private readonly Guid? _currentUserId = userContextService.GetCurrentUserId();

    public async Task<BasePostResponse> CreateFriendship(CreateFriendshipRequest req)
    {
        if (_currentUserId is null)
        {
            return new BasePostResponse(false, Message: "Unable to access User Id");
        }

        var createResult = await write.CreateAsync(req, _currentUserId.Value);

        return new BasePostResponse(createResult, createResult ? "Success" : "Error requesting friendship");
    }
}