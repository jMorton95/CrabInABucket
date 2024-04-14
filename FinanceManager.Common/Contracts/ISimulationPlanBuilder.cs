using FinanceManager.Common.Models;

namespace FinanceManager.Common.Contracts;

public interface ISimulationPlanBuilder
{
    SimulationPlan CreateSimulationPlan(SimulationParameters simulationParameters);
}
