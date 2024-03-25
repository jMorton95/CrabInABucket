using FinanceManager.Common.DataEntities;
using FinanceManager.Common.Middleware.UserContext;
using FinanceManager.Common.Requests;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Write.Friendships;

public interface IWriteFriendships
{
    Task<bool> CreateAsync(Guid userId, Guid targetUserId);
    Task<bool> EditAsync(Guid friendshipId, bool accepted);
}

public class WriteFriendships(DataContext db) : IWriteFriendships
{
    public async Task<bool> CreateAsync(Guid userId, Guid targetUserId)
    {
        var friendship = new Friendship();
        
        await db.AddAsync(friendship);

        List<Guid> userFriendshipIds = [userId, targetUserId];
        
        await db.AddRangeAsync(
            userFriendshipIds.Select(x => new UserFriendship { UserId = x, Friendship = friendship}
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

    public async Task<bool> EditAsync(Guid friendshipId, bool accepted)
    {
        var friendship = await db.Friendship.FirstOrDefaultAsync(x => x.Id == friendshipId);

        if (friendship is null) return false;
        
        friendship.IsPending = false;
        friendship.IsAccepted = accepted;

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