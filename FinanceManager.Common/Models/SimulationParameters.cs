namespace FinanceManager.Common.Models;

public record Range(int Min, int Max);

public record Users(Range Count, decimal StartingBalanceBias);

public record Friendships(Range FriendsPerUserRange);

public record Accounts(Range Count, decimal MaxStartingBalance);

public record SimulationParameters(
    bool ShouldSimulate,
    int Duration,
    int MaxStartingBalance,
    Users Users,
    Friendships Friendships,
    Accounts Accounts
);