using CrabInABucket.DataContext.Models;
using Microsoft.EntityFrameworkCore;

namespace CrabInABucket.DataContext;

public class CrabDbContext : DbContext
{
    public CrabDbContext(DbContextOptions options) : base(options) { }

    public DbSet<User> User => Set<User>();
    public DbSet<Role> Role => Set<Role>();
    public DbSet<UserRole> UserRole => Set<UserRole>();
    public DbSet<Account> Account => Set<Account>();
    public DbSet<BudgetTransaction> BudgetTransaction => Set<BudgetTransaction>();
}