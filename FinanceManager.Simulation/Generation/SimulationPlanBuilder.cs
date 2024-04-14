using FinanceManager.Common.Contracts;
using FinanceManager.Common.Models;

namespace FinanceManager.Simulation.Generation;



public class SimulationPlanBuilder : ISimulationPlanBuilder
{
    public Task<SimulationPlan> CreateSimulationPlan(SimulationParameters simulationParameters)
    {
        var totalUsersToSimulate = SimulationHelpers.GetNumberFromRange(simulationParameters.Users.Count);
        return Task.FromResult(new SimulationPlan(totalUsersToSimulate / simulationParameters.Duration));
    }
}

