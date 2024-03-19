using FinanceManager.Core.DataEntities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Read.Friends;

public interface IReadUserFriends
{
    Task<bool> CheckUsersAreFriends(Guid requesterId, Guid targetId);
    Task<List<User>> GetUserFriends(Guid userId);
    Task<List<User>> GetRelatedFriends(Guid userId);
    Task<List<User>> GetRandomFriendSuggestions(Guid userId, int amountOfSuggestions);
}

public class ReadUserFriends(DataContext db) : IReadUserFriends
{
    public async Task<bool> CheckUsersAreFriends(Guid requesterId, Guid targetId)
    {
        var userFriendshipStatus = await db.Friendship
            .AnyAsync(f => f.IsAccepted && 
                           f.UserFriendships.Any(uf => uf.UserId == requesterId) && 
                           f.UserFriendships.Any(uf => uf.UserId == targetId));
        
        return userFriendshipStatus;
    }
    public async Task<List<User>> GetUserFriends(Guid userId)
    {
        var users = await db.UserFriendship
            .Where(uf => uf.Friendship.UserFriendships.Any(f => f.UserId == userId) && uf.UserId != userId && uf.Friendship.IsAccepted == true)
            .Select(uf => uf.User)
            .Distinct()
            .ToListAsync();
        
        return users;
    }
    
    public async Task<List<User>> GetRelatedFriends(Guid userId)
    {
        var friends = await db.UserFriendship
            .Where(uf => uf.Friendship.UserFriendships
                .Any(f => f.UserId == userId) && uf.UserId != userId && uf.Friendship.IsAccepted == true)
            .Distinct()
            .ToListAsync();
        
        var friendIds = friends
            .Select(f => f.UserId)
            .ToList();
        
        var friendsOfFriends = await db.UserFriendship
            .Where(uf => friendIds
                .Any(id => uf.UserId != id) && uf.UserId != userId)
            .Select(x => x.User)
            .Distinct()
            .ToListAsync();

        return friendsOfFriends;
    }

    public async Task<List<User>> GetRandomFriendSuggestions(Guid userId, int amountOfSuggestions)
    {
        var friendIds = await db.UserFriendship
            .Where(uf => uf.UserId == userId)
            .Select(uf => uf.UserId)
            .ToListAsync();
        
        friendIds.Add(userId);
        
        var potentialSuggestions = await db.User
            .Where(u => !friendIds.Contains(u.Id))
            .OrderBy(u => Guid.NewGuid())
            .Take(amountOfSuggestions)
            .ToListAsync();

        return potentialSuggestions;
    }
}