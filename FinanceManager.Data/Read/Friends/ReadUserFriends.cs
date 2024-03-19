using FinanceManager.Core.DataEntities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Read.Friends;

public interface IReadUserFriends
{
    Task<List<User>> GetUserFriends(Guid userId);
    Task<List<User>> GetRelatedFriends(Guid userId);
    Task<bool> CheckUsersAreFriends(Guid requesterId, Guid targetId);
}

public class ReadUserFriends(DataContext db) : IReadUserFriends
{
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

    public async Task<bool> CheckUsersAreFriends(Guid requesterId, Guid targetId)
    {
        var userFriendshipStatus = await db.Friendship
            .AnyAsync(f => f.IsAccepted && 
                           f.UserFriendships.Any(uf => uf.UserId == requesterId) && 
                           f.UserFriendships.Any(uf => uf.UserId == targetId));
        
        return userFriendshipStatus;
    }
}