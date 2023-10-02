using FinanceManager.Core.AppConstants;
using FinanceManager.Core.Models;
using FinanceManager.Data.Write.Generic;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Write.Users;

public interface IWriteUsers : IWrite<User>
{
    Task<int> GrantAdministratorRole(User user);
}

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

    public async Task<int> GrantAdministratorRole(User user)
    {
        var adminRole = await db.Role.FirstOrDefaultAsync(x => x.Name == RoleConstants.AdminRole);

        if (adminRole == null)
        {
            adminRole = new Role() { Name = RoleConstants.AdminRole };
            db.Role.Add(adminRole);
        }
        
        db.UserRole.Add(new UserRole(){User = user, Role = adminRole});

        return await db.SaveChangesAsync();
    }
}