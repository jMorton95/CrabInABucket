using CrabInABucket.Data.Models;
using CrabInABucket.Data.Write.Generic;

namespace CrabInABucket.Data.Write.Users;

public interface IWriteUsers : IWrite<User>;

public sealed class WriteUsers(DataContext db) : IWriteUsers
{
    public async Task<int> CreateAsync(User entity)
    {
        await db.User.AddAsync(entity);

        return await db.SaveChangesAsync();
    }

    public async Task<int> EditAsync(User entity)
    {
        db.User.Update(entity);

        return await db.SaveChangesAsync();
    }
}