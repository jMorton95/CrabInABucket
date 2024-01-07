using FinanceManager.Core.AppConstants;
using FinanceManager.Data;
using FinanceManager.Data.Read.Users;
using FinanceManager.Data.Write.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Application.DependencyInjection;

public static class DataRegistration
{
    public static void AddPostgresConnection(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString(SettingsConstants.PostgresConnection);
        builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));
    }

    public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
    {
        services.AddScoped<IReadUsers, ReadUsers>();
        services.AddScoped<IWriteUsers, WriteUsers>();

        return services;
    }
}