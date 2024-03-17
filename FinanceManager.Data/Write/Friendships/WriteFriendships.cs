using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Middleware.UserContext;
using FinanceManager.Core.Requests;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Write.Friendships;

public interface IWriteFriendships : IEditEntity<AcceptFriendshipRequest>
{
    Task<bool> CreateAsync(CreateFriendshipRequest request, Guid currentUserId);
}

public class WriteFriendships(DataContext db, IUserContextService userContextService) : IWriteFriendships
{
    private readonly Guid? _currentUserId = userContextService.GetCurrentUserId();
    
    public async Task<bool> CreateAsync(CreateFriendshipRequest request, Guid currentUserId)
    {
        var friendship = new Friendship();
        await db.AddAsync(friendship);
        
        await db.AddRangeAsync(
            new List<Guid> { currentUserId, request.TargetUserId }
                .Select(x => new UserFriendship { UserId = x, Friendship = friendship}
            ));

        try
        {
            var result = await db.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            //TODO: Handle Logging
            return false;
        }
    }

    public async Task<bool> EditAsync(AcceptFriendshipRequest request)
    {
        if (_currentUserId is null) return false;

        var friendship = await db.Friendship.FirstOrDefaultAsync(x => x.Id == request.FriendshipId);

        if (friendship is null) return false;
        
        friendship.IsPending = false;
        friendship.IsAccepted = request.Accepted;

        try
        {
            var result = await db.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            //TODO: Handle Logging
            return false;
        }
    }
}