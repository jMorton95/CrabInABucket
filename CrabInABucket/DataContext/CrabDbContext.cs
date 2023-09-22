using CrabInABucket.DataContext.Models;
using Microsoft.EntityFrameworkCore;

namespace CrabInABucket.DataContext;

public class CrabDbContext : DbContext
{
    public CrabDbContext(DbContextOptions options) : base(options) { }

    public DbSet<User> User => Set<User>();
}