using FinanceManager.Services.Processes;
using FinanceManager.Services.Processes.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Application.DependencyInjection;

public static class ProcessRegistration
{
    public static IServiceCollection AddProcesses(this IServiceCollection services)
    {
        services.AddScoped<IPasswordProcess, PasswordProcess>();

        return services;
    }
}