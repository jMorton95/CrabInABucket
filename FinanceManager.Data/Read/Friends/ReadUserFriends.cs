using FinanceManager.Core.DataEntities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Read.Friends;

public interface IReadUserFriends
{
    Task<bool> CheckUsersAreFriends(Guid requesterId, Guid targetId);
    Task<List<User>> GetUserFriends(Guid userId);
    Task<List<User>> GetRelatedFriends(Guid userId);
    Task<List<User>> GetRandomFriendSuggestions(Guid userId, int amountOfSuggestions);
    Task<List<User>> GetPendingFriendRequests(Guid userId);
}

public class ReadUserFriends(DataContext db) : IReadUserFriends
{
    public async Task<bool> CheckUsersAreFriends(Guid requesterId, Guid targetId)
    {
        var userFriendshipStatus = await db.Friendship
            .AsNoTracking()
            .AnyAsync(f => f.IsAccepted && 
                           f.UserFriendships.Any(uf => uf.UserId == requesterId) && 
                           f.UserFriendships.Any(uf => uf.UserId == targetId));
        
        return userFriendshipStatus;
    }
    public async Task<List<User>> GetUserFriends(Guid userId)
    {
        var users = await db.UserFriendship
            .AsNoTracking()
            .Where(uf => uf.Friendship.UserFriendships.Any(f => f.UserId == userId) && uf.UserId != userId && uf.Friendship.IsAccepted == true)
            .Select(uf => uf.User)
            .Distinct()
            .ToListAsync();
        
        return users;
    }
    
    public async Task<List<User>> GetRelatedFriends(Guid userId)
    {
        var userFriendships = await db.Friendship
            .AsNoTracking()
            .Where(x => x.UserFriendships
                .Any(y => y.UserId == userId))
            .SelectMany(f => f.UserFriendships)
            .Where(x => x.UserId != userId)
            .Select(uf => uf.UserId)
            .Distinct()
            .ToListAsync();

        var friendsOfFriends = await db.Friendship
            .AsNoTracking()
            .Where(f => 
                f.UserFriendships.Any(uf => userFriendships.Contains(uf.UserId))
                && f.UserFriendships.Any(uf => uf.UserId != userId))
            .SelectMany(x => x.UserFriendships)
            .Select(x => x.UserId)
            .Where(y => y != userId && !userFriendships.Contains(y))
            .Distinct()
            .ToListAsync();

        var users = await db.User.Where(u => friendsOfFriends.Contains(u.Id))
            .ToListAsync();
        
        return users;
    }

    public async Task<List<User>> GetRandomFriendSuggestions(Guid userId, int amountOfSuggestions)
    {
        var friendIds = await db.UserFriendship
            .AsNoTracking()
            .Where(uf => uf.UserId == userId)
            .Select(uf => uf.UserId)
            .Distinct()
            .ToListAsync();
        
        friendIds.Add(userId);
        
        var potentialSuggestions = await db.User
            .AsNoTracking()
            .Where(u => !friendIds.Contains(u.Id))
            .OrderBy(u => Guid.NewGuid())
            .Take(amountOfSuggestions)
            .Distinct()
            .ToListAsync();

        return potentialSuggestions;
    }

    public async Task<List<User>> GetPendingFriendRequests(Guid userId)
    {
        var pendingUsers = await db.UserFriendship
            .AsNoTracking()
            .Where(uf => uf.Friendship.IsPending && uf.Friendship.UserFriendships.Any(x => x.UserId == userId) && uf.UserId != userId)
            .Select(x => x.User)
            .Distinct()
            .ToListAsync();
        
        return pendingUsers;
    }
}