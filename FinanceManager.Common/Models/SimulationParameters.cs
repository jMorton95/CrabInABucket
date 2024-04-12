namespace FinanceManager.Common.Models;

public record Range(int Min, int Max);

public record Users(Range Count, decimal StartingBalanceBias);

public record Friendships(Range FriendsPerUserRange);

public record Accounts(Range Count, decimal MaxStartingBalance);

public class SimulationParameters
{
    public bool ShouldSimulate { get; init; }
    public int Duration { get; init; }
    public int MaxStartingBalance { get; init; }
    public Users Users { get; init; }
    public Friendships Friendships { get; init; }
    public Accounts Accounts { get; init; } 
}