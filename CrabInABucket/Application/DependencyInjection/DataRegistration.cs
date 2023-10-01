using CrabInABucket.Data.Read.Users;
using Microsoft.EntityFrameworkCore;

namespace CrabInABucket.Application.DependencyInjection;

public static class DataRegistration
{
    public static void AddPostgres<TContext>(this IServiceCollection services, string connectionString) where TContext : DbContext
    {
        services.AddDbContext<TContext>(options => options.UseNpgsql(connectionString));
    }

    public static IServiceCollection AddReaders(this IServiceCollection services)
    {
        services.AddScoped<IReadUsers, ReadUsers>();

        return services;
    }
}