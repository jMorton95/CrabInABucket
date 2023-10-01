using CrabInABucket.Core.Workers;
using CrabInABucket.Core.Workers.Interfaces;

namespace CrabInABucket.Application.DependencyInjection;
public static class WorkerRegistration
{
    public static IServiceCollection AddWorkers(this IServiceCollection services)
    {
        services.AddScoped<ILoginWorker, LoginWorker>();

        return services;
    }
}