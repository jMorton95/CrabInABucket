using FinanceManager.Common.Models;

namespace FinanceManager.Common.Contracts;

public interface ISimulationPlanBuilder
{
    Task<SimulationPlan> CreateSimulationPlan(SimulationParameters simulationParameters);
}
