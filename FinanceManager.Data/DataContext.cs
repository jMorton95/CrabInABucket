using FinanceManager.Common.Entities;
using FinanceManager.Common.Services;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data;

public class DataContext(DbContextOptions<DataContext> options, IUserContextService userContextService) : DbContext(options)
{
    public DbSet<User> User => Set<User>();
    public DbSet<Role> Role => Set<Role>();
    public DbSet<UserRole> UserRole => Set<UserRole>();
    public DbSet<Account> Account => Set<Account>();
    public DbSet<RecurringTransaction> RecurringTransaction => Set<RecurringTransaction>();
    public DbSet<Transaction> Transaction => Set<Transaction>();
    public DbSet<Friendship> Friendship => Set<Friendship>();
    public DbSet<UserFriendship> UserFriendship => Set<UserFriendship>();
    public DbSet<Settings> Settings => Set<Settings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        
        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();
        
        modelBuilder.Entity<Friendship>()
            .HasMany(f => f.UserFriendships)
            .WithOne(uf => uf.Friendship)
            .HasForeignKey(uf => uf.FriendshipId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Friendship_Users");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var userId = userContextService.CurrentUser?.UserAccessToken?.UserId;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is not Entity entity) continue;

            if (userId != null) entity.EditedBy = userId;
            entity.UpdatedDate = DateTime.UtcNow;

            switch (entry)
            {
                case { State: EntityState.Added }:
                    if (userId != null) entity.CreatedBy = userId;
                    break;

                case { State: EntityState.Modified }:
                    entity.RowVersion += 1;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}