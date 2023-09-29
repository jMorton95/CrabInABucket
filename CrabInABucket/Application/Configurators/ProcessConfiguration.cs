using CrabInABucket.Core.Processes;
using CrabInABucket.Core.Processes.Interfaces;

namespace CrabInABucket.Core.Configurators;

public static class ProcessConfiguration
{
    public static void AddProcesses(this IServiceCollection services)
    {
        services.AddScoped<IPasswordProcess, PasswordProcess>();
    }
}