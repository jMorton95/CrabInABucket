namespace FinanceManager.Common.Models;

public record ParameterRange(int Min, int Max);

public record Users(ParameterRange Count, decimal StartingBalanceBias);

public record Friendships(ParameterRange FriendsPerUserParameterRange);

public record Accounts(ParameterRange Count, decimal MaxStartingBalance);

public class SimulationParameters
{
    public bool ShouldSimulate { get; init; }
    public int Duration { get; init; }
    public int MaxStartingBalance { get; init; }
    public Users Users { get; init; }
    public Friendships Friendships { get; init; }
    public Accounts Accounts { get; init; } 
}