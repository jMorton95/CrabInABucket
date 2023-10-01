using FinanceManager.Data.Read.Users;
using FinanceManager.Data.Write.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Application.DependencyInjection;

public static class DataRegistration
{
    public static void AddPostgres<TContext>(this IServiceCollection services, string connectionString) where TContext : DbContext
    {
        services.AddDbContext<TContext>(options => options.UseNpgsql(connectionString));
    }

    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IReadUsers, ReadUsers>();
        services.AddScoped<IWriteUsers, WriteUsers>();

        return services;
    }
}