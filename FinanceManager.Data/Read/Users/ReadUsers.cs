using FinanceManager.Core.DataEntities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Read.Users;

public interface IReadUsers : IGetEntityByIdAsync<User>, IGetAllEntitiesAsync<User>
{
    Task<bool> CheckUserExistsByEmail(string emailAddress);
    Task<User?> GetUserByEmailAsync(string emailAddress);
    Task<List<User>> GetUserFriends(Guid userId);
}

public sealed class ReadUsers(DataContext db) : IReadUsers
{
    public async Task<IEnumerable<User>> GetAllAsync()
        => await db.User
            .Include(x => x.Roles)
            .ThenInclude(r => r.Role)
            .ToListAsync();
    
    public async Task<User?> GetByIdAsync(Guid id)
        => await db.User
            .Include(x => x.Roles)
            .ThenInclude(r => r.Role)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<bool> CheckUserExistsByEmail(string emailAddress)
        => await db.User
            .AnyAsync(x => x.Username == emailAddress);
    
    public async Task<User?> GetUserByEmailAsync(string emailAddress)
        => await db.User
            .Include(x => x.Roles)
            .ThenInclude(r => r.Role)
            .FirstOrDefaultAsync(x => x.Username == emailAddress);

    public async Task<List<User>> GetUserFriends(Guid userId)
    {
        var users = await db.UserFriendship
            .Where(uf => uf.Friendship.UserFriendships.Any(f => f.UserId == userId) && uf.UserId != userId)
            .Select(uf => uf.User)
            .Distinct()
            .ToListAsync();
        
        return users;
    }
}