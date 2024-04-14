using FinanceManager.Common.Models;

namespace FinanceManager.Simulation;

public static class SimulationHelpers
{
    private static readonly Random Random = new();
    public static int GetNumberFromRange(ParameterRange range) 
        => Random.Next(range.Min, range.Max);
    
    private static double GenerateNormalDistribution(double mean, double stdDev)
    {
        var u1 = 1.0 - Random.NextDouble();
        var u2 = 1.0 - Random.NextDouble();
        var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                               Math.Sin(2.0 * Math.PI * u2);
        var randNormal = mean + stdDev * randStdNormal;
        return randNormal;
    }
    
    public static decimal GetWeightedRandomNumber(int min, int max, double meanPercentage)
    {
        var range = max - min;
        var mean = min + range * meanPercentage;
        var stdDev = range / 6;

      
        var result = GenerateNormalDistribution(mean, stdDev);

        return Convert.ToDecimal(result < min ? min : result > max ? max : result);
    }
}