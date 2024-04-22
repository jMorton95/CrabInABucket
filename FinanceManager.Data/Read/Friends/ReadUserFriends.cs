using System.Collections;
using FinanceManager.Common.Entities;
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


public static class StaticReadUserFriends
{
    public static async Task<Dictionary<Guid, List<Guid>>> StatsicGetRandomSuggestions(this DataContext db, List<Guid> userIds, int amountOfSuggestions)
    {
        var result = db.UserFriendship
            .Include(uf => uf.Friendship)
            .ThenInclude(f => f.UserFriendships)
            .AsEnumerable()
            .Where(uf => userIds.Contains(uf.UserId))
            .SelectMany(uf => uf.Friendship.UserFriendships, (uf, friend) => new { uf.UserId, FriendUserId = friend.UserId })
            .Where(x => x.UserId != x.FriendUserId)
            .Distinct();
        
            var tings = userIds.Select(id => new { UserId = id, FriendIds = result.Where(y => y.UserId == id).Select(y => y.FriendUserId) });
      
            return tings.ToDictionary(x => x.UserId, x => x.FriendIds.ToList());
    }
    public static async Task<List<User>> StaticGetRandomSuggestions(this DataContext db, Guid userId, int amountOfSuggestions)
    {
        var userFriendIds = await db.Friendship
            .AsNoTracking()
            .Where(x => x.UserFriendships
                .Any(y => y.UserId == userId))
            .SelectMany(f => f.UserFriendships)
            .Where(x => x.UserId != userId)
            .Select(uf => uf.UserId)
            .Distinct()
            .ToListAsync();
        
        var potentialSuggestions = await db.User
            .AsNoTracking()
            .Where(u => !userFriendIds.Contains(u.Id) && u.Id != userId)
            .OrderBy(u => Guid.NewGuid())
            .Take(amountOfSuggestions)
            .Distinct()
            .ToListAsync();
    
        return potentialSuggestions;
    }
}
public class ReadUserFriends(DataContext db) : IReadUserFriends
{
    private async Task<List<Guid>> GetUserFriendIds(Guid userId)
    {
        return await db.Friendship
            .AsNoTracking()
            .Where(x => x.UserFriendships
                .Any(y => y.UserId == userId))
            .SelectMany(f => f.UserFriendships)
            .Where(x => x.UserId != userId)
            .Select(uf => uf.UserId)
            .Distinct()
            .ToListAsync();
    }
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
            .Where(uf => uf.Friendship.UserFriendships
                .Any(f => f.UserId == userId) && uf.UserId != userId && uf.Friendship.IsAccepted == true)
            .Select(uf => uf.User)
            .Distinct()
            .ToListAsync();
        
        return users;
    }
    
    public async Task<List<User>> GetRelatedFriends(Guid userId)
    {
        var userFriendIds = await GetUserFriendIds(userId);

        var friendsOfFriends = await db.Friendship
            .AsNoTracking()
            .Where(f => 
                f.UserFriendships.Any(uf => userFriendIds.Contains(uf.UserId))
                && f.UserFriendships.Any(uf => uf.UserId != userId))
            .SelectMany(x => x.UserFriendships)
            .Select(x => x.UserId)
            .Where(y => y != userId && !userFriendIds.Contains(y))
            .Distinct()
            .ToListAsync();

        var users = await db.User
            .Where(u => friendsOfFriends
                .Contains(u.Id))
            .ToListAsync();
        
        return users;
    }

    public async Task<List<User>> GetRandomFriendSuggestions(Guid userId, int amountOfSuggestions)
    {
        var userFriendIds = await GetUserFriendIds(userId);
        
        var potentialSuggestions = await db.User
            .AsNoTracking()
            .Where(u => !userFriendIds.Contains(u.Id) && u.Id != userId)
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
            .Where(uf => uf.Friendship.IsPending && uf.Friendship.UserFriendships
                .Any(x => x.UserId == userId) && uf.UserId != userId)
            .Select(x => x.User)
            .Distinct()
            .ToListAsync();
        
        return pendingUsers;
    }
}