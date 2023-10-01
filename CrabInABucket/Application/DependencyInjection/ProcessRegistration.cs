using CrabInABucket.Core.Processes;
using CrabInABucket.Core.Processes.Interfaces;

namespace CrabInABucket.Application.DependencyInjection;

public static class ProcessRegistration
{
    public static void AddProcesses(this IServiceCollection services)
    {
        services.AddScoped<IPasswordProcess, PasswordProcess>();
    }
}