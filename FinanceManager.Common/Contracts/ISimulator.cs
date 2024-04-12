using FinanceManager.Common.Models;

namespace FinanceManager.Common.Contracts;

public interface ISimulator
{
    Task<bool> RunSimulation(SimulationParameters parameters);
    Task<bool> SimulateFromConfiguration(SimulationParameters simulationParameters);
    Task<bool> RemoveSimulatedData();
}