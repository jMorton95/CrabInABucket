using FinanceManager.Common.Contracts;
using FinanceManager.Common.Models;

namespace FinanceManager.Simulation;



public class SimulationPlanBuilder : ISimulationPlanBuilder
{
    public SimulationPlan CreateSimulationPlan(SimulationParameters simulationParameters)
    {
        var totalUsersToSimulate = SimulationHelpers.GetNumberFromRange(simulationParameters.Users.Count);
        return new SimulationPlan(totalUsersToSimulate / simulationParameters.Duration);
    }
}

