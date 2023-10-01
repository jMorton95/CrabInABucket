using FinanceManager.Api.Core.Processes;
using FinanceManager.Api.Core.Processes.Interfaces;

namespace FinanceManager.Application.DependencyInjection;

public static class ProcessRegistration
{
    public static IServiceCollection AddProcesses(this IServiceCollection services)
    {
        services.AddScoped<IPasswordProcess, PasswordProcess>();

        return services;
    }
}