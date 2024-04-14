using FinanceManager.Common.Models;

namespace FinanceManager.Common.Contracts;

public interface ISimulator
{
    Task<bool> StartSimulation(Entities.Settings settings);
    Task<bool> SimulateFromConfiguration();
    Task<bool> RemoveSimulatedData(Entities.Settings settings);
}