using CrabInABucket.Core.Services;
using CrabInABucket.Core.Services.Interfaces;

namespace CrabInABucket.Application.DependencyInjection;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();
    }
}