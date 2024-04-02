using FinanceManager.Common.Constants;
using FinanceManager.Common.Entities;
using FinanceManager.Common.Requests;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Write.Users;

public interface IWriteUsers :
    ICreateEntity<User>,
    IEditEntity<EditUserRequest>
{
    Task<bool> UpdateLastLogin(User user);
    Task<int> ManageUserAdministratorRole(User user, bool isAdmin);
}

public sealed class WriteUsers(DataContext db) : IWriteUsers
{
    public async Task<bool> CreateAsync(User entity)
    {
        db.User.Add(entity);

        var saveResult = await db.SaveChangesAsync().ConfigureAwait(false);
        
        return saveResult > 0;
    }

    public async Task<bool> EditAsync(EditUserRequest req)
    {
        //db.User.Update(entity);
        return true;
        //eturn await db.SaveChangesAsync();
    }

    private Role CreateAdministratorRole()
    {
        var adminRole = new Role() { Name = PolicyConstants.AdminRole };
        db.Role.Add(adminRole);

        return adminRole;
    }

    public async Task<bool> UpdateLastLogin(User user)
    {
        user.LastOnline = DateTime.UtcNow;

        try
        {
            var result = await db.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
            //TODO: Integrate Logger
        }
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