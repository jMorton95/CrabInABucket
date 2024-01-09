using FinanceManager.Core.AppConstants;
using FinanceManager.Core.DataEntities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Write.Users;

public interface IWriteUsers : IWrite<User>
{
    Task<int> ManageUserAdministratorRole(User user, bool isAdmin);
}

public sealed class WriteUsers(DataContext db) : IWriteUsers
{
    public async Task<int> CreateAsync(User entity)
    {
        db.User.Add(entity);
        
        return await db.SaveChangesAsync();
    }

    public async Task<int> EditAsync(User entity)
    {
        db.User.Update(entity);

        return await db.SaveChangesAsync();
    }

    private Role CreateAdministratorRole()
    {
        var adminRole = new Role() { Name = PolicyConstants.AdminRole };
        db.Role.Add(adminRole);

        return adminRole;
    }
    
    public async Task<int> ManageUserAdministratorRole(User user, bool isAdmin)
    {
        var adminRole = await db.Role.FirstOrDefaultAsync(x => x.Name == PolicyConstants.AdminRole) ?? CreateAdministratorRole();

        if (isAdmin)
        {
            db.UserRole.Add(new UserRole() { User = user, Role = adminRole });
        }
        else
        {
            user.Roles = user.Roles.Where(x => x.Role?.Id == adminRole.Id);    
        }
        
        return await db.SaveChangesAsync();
    }
}