using FinanceManager.Common.Models;

namespace FinanceManager.Common.Contracts;

public interface ISimulator
{
    Task<bool> RunSimulation(SimulationParameters parameters);
}