using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Middleware.UserContext;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Read;

public interface IReadFriendships : IGetEntityByIdAsync<Friendship>
{
    Task<bool> DoesFriendshipExist(Guid targetUserId);
}

public class ReadFriendships(DataContext db, IUserContextService userContextService) : IReadFriendships
{
    private readonly Guid? _currentUserId = userContextService.GetCurrentUserId();
    
    public async Task<Friendship?> GetByIdAsync(Guid id)
    {
        var result = await db.Friendship.FirstOrDefaultAsync(x => x.Id == id);

        return result;
    }

    public async Task<bool> DoesFriendshipExist(Guid targetUserId)
    {
        if (_currentUserId is null) return false;
        
        var result = await db.Friendship
            .AnyAsync(f => f.UserFriendships.Any(uf => uf.UserId == _currentUserId) &&
                           f.UserFriendships.Any(uf => uf.UserId == targetUserId));

        return result;
    }
}