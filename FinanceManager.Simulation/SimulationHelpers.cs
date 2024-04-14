using FinanceManager.Common.Models;

namespace FinanceManager.Simulation;

public static class SimulationHelpers
{
    public static int GetNumberFromRange(ParameterRange range) 
        => new Random().Next(range.Min, range.Max);
}