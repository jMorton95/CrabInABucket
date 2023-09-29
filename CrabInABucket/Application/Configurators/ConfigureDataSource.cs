using Microsoft.EntityFrameworkCore;

namespace CrabInABucket.Data;

public static class ConfigureDataSource
{
    public static void AddPostgres<TContext>(this IServiceCollection services, string connectionString) where TContext : DbContext
    {
        services.AddDbContext<TContext>(options => options.UseNpgsql(connectionString));
    }
}