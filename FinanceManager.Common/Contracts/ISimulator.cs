using FinanceManager.Common.Models;

namespace FinanceManager.Common.Contracts;

public interface ISimulator
{
    Task<bool> StartSimulation(SimulationParameters parameters, Entities.Settings settings);
    Task<bool> SimulateFromConfiguration(SimulationParameters simulationParameters);
    Task<bool> RemoveSimulatedData(Entities.Settings settings);
}