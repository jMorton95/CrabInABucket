using CrabInABucket.Core.Services;
using CrabInABucket.Core.Services.Interfaces;

namespace CrabInABucket.Core.Configurators;

public static class ServiceConfiguration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
    }
}