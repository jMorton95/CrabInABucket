namespace FinanceManager.Common.Models;

public record ParameterRange(int Min, int Max);

public record Users(ParameterRange Count);

public record Friendships(ParameterRange FriendsPerUser);

public record Accounts(ParameterRange Count, int MaxStartingBalance, double StartingBalanceBias);

public class SimulationParameters
{
    public bool ShouldOverwrite { get; set; }
    public bool ShouldSimulate { get; init; }
    public int Duration { get; init; }
    public int MaxStartingBalance { get; init; }
    public Users Users { get; init; }
    public Friendships Friendships { get; init; }
    public Accounts Accounts { get; init; } 
}