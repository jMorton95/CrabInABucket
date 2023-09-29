using CrabInABucket.Core.Workers;
using CrabInABucket.Core.Workers.Interfaces;

namespace CrabInABucket.Core.Configurators;
public static class WorkerConfiguration
{
    public static void AddWorkers(this IServiceCollection services)
    {
        services.AddScoped<ILoginWorker, LoginWorker>();
    }
}