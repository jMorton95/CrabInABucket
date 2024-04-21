using FinanceManager.Common.Contracts;
using FinanceManager.Common.Models;
using FinanceManager.Common.Services;
using FinanceManager.Data;
using FinanceManager.Data.Read.Friends;
using FinanceManager.Data.Read.Friendships;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Simulation;

public interface ISimulatorFactory
{
    ISimulator CreateSimulator(SimulationParameters simulationParameters);
}

public class SimulatorFactory(
    DataContext db,
    IDbContextFactory<DataContext> dbContextFactory,
    IPasswordHasher hasher,
    ISimulationPlanBuilder builder,
    IReadUserFriends readUserFriends) : ISimulatorFactory
{
    public ISimulator CreateSimulator(SimulationParameters simulationParameters)
    {
        return new Simulator(simulationParameters, db, dbContextFactory, hasher, builder, readUserFriends);
    }
}