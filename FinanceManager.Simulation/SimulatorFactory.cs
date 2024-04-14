using FinanceManager.Common.Contracts;
using FinanceManager.Common.Models;
using FinanceManager.Common.Services;
using FinanceManager.Data;

namespace FinanceManager.Simulation;

public interface ISimulatorFactory
{
    ISimulator CreateSimulator(SimulationParameters simulationParameters);
}

public class SimulatorFactory(DataContext db, IPasswordHasher hasher, ISimulationPlanBuilder builder) : ISimulatorFactory
{
    public ISimulator CreateSimulator(SimulationParameters simulationParameters)
    {
        return new Simulator(simulationParameters, db, hasher, builder);
    }
}