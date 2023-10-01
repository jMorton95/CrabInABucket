using CrabInABucket.Core.Services.Interfaces;
using CrabInABucket.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CrabInABucket.Data;

public class DataContext(DbContextOptions options, IUserAccessorService userAccessorService) : DbContext(options)
{
    public DbSet<User> User => Set<User>();
    public DbSet<Role> Role => Set<Role>();
    public DbSet<UserRole> UserRole => Set<UserRole>();
    public DbSet<Account> Account => Set<Account>();
    public DbSet<BudgetTransaction> BudgetTransaction => Set<BudgetTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
    }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var userId = userAccessorService.GetCurrentUserId();
        
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is not BaseModel entity ) continue;

            entity.EditedBy = userId;
            entity.UpdatedDate = DateTime.UtcNow;

            switch (entry)
            {
                case { State: EntityState.Added }:
                    entity.CreatedBy = userId;
                    break;
                
                case { State: EntityState.Modified }:
                    entity.RowVersion += 1;
                    break;
            }
        }
        
        
        return base.SaveChangesAsync(cancellationToken);
    }


}