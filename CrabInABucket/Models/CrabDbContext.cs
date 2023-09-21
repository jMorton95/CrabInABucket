using Microsoft.EntityFrameworkCore;

namespace CrabInABucket.Models;

public class CrabDbContext : DbContext
{
    public CrabDbContext(DbContextOptions options) : base(options) { }
}