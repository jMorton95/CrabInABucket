using CrabInABucket.Data.Models;
using CrabInABucket.Data.Write.Generic;

namespace CrabInABucket.Data.Write.Users;

public interface IWriteUsers : IWrite<User>;

public sealed class WriteUsers(DataContext db) : IWriteUsers
{
    public async Task<int> CreateAsync(User entity)
    {
        var userToAdd = db.User.Add(entity);
        
        var userId = userToAdd.Entity.Id;
        
        userToAdd.Entity.CreatedBy = userId;
        userToAdd.Entity.EditedBy = userId;
        
        return await db.SaveChangesAsync();
    }

    public async Task<int> EditAsync(User entity)
    {
        db.User.Update(entity);

        return await db.SaveChangesAsync();
    }
}