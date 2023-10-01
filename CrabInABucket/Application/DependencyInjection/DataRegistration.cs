using Microsoft.EntityFrameworkCore;

namespace CrabInABucket.Application.DependencyInjection;

public static class DataRegistration
{
    public static void AddPostgres<TContext>(this IServiceCollection services, string connectionString) where TContext : DbContext
    {
        services.AddDbContext<TContext>(options => options.UseNpgsql(connectionString));
    }
}