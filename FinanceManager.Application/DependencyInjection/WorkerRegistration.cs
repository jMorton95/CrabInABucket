using FinanceManager.Services.Workers;
using FinanceManager.Services.Workers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Application.DependencyInjection;

public static class WorkerRegistration
{
    public static IServiceCollection AddWorkers(this IServiceCollection services)
    {
        services.AddScoped<ILoginWorker, LoginWorker>();
        services.AddScoped<ICreateUserWorker, CreateUserWorker>();

        return services;
    }
}