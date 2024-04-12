namespace FinanceManager.Simulation.Generation;

public record Range(decimal Min, decimal Max);

public record Users(Range Count, decimal StartingBalanceBias);

public record Friendships(Range FriendsPerUserRange);

public record Accounts(Range Count, decimal MaxStartingBalance);

public record Parameters (
    int Duration,
    int MaxStartingBalance,
    Users Users,
    Friendships Friendships,
    Accounts Accounts
);