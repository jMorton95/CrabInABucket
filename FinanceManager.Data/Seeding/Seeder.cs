using FinanceManager.Common.Constants;
using FinanceManager.Common.Entities;
using FinanceManager.Common.Services;
using FinanceManager.Common.Settings;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Seeding;

public static class Seeder
{
    public static async Task InsertTestData(this DataContext dataContext)
    {
        
    }

    public static async Task InsertProductionData(this DataContext dataContext, SuperAdminSettings settings, PasswordHasher passwordHasher)
    {
        var isAdminCreated = await dataContext.User.AnyAsync(x => x.Username == settings.Email);

        if (isAdminCreated)
        {
            return;
        }

        var adminRole = await dataContext.Role.FirstOrDefaultAsync(x => x.Name == PolicyConstants.AdminRole);

        if (adminRole == null)
        {
            adminRole = new Role { Name = PolicyConstants.AdminRole };
            dataContext.Add(adminRole);
        }

        var hashedPassword = passwordHasher.HashPassword(settings.Password);
        var newSuperAdmin = new User { Username = settings.Email,
            Password = hashedPassword,
            Roles = [new UserRole{Role = adminRole}]
        };

        dataContext.Add(newSuperAdmin);
        await dataContext.SaveChangesAsync();
    }
}