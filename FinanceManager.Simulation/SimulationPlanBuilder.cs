using FinanceManager.Common.Contracts;
using FinanceManager.Common.Models;

namespace FinanceManager.Simulation;

public class SimulationPlanBuilder : ISimulationPlanBuilder
{
    public SimulationPlan CreateSimulationPlan(SimulationParameters simulationParameters)
    {
        var totalUsersToSimulate = SimulationHelpers.GetNumberFromRange(simulationParameters.Users.Count);
        var maxFriendsPerUser = SimulationHelpers.GetNumberFromRange(simulationParameters.Friendships.FriendsPerUser);
        
        return new SimulationPlan
        (
            TotalUsersToSimulate: totalUsersToSimulate,
            MaxFriendsPerUser: maxFriendsPerUser,
            UsersPerTick: totalUsersToSimulate / simulationParameters.Duration,
            MaxFriendsPerTick: maxFriendsPerUser / simulationParameters.Duration
        );
    }
}

