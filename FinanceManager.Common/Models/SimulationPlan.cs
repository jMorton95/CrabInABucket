namespace FinanceManager.Common.Models;

public record SimulationPlan(int TotalUsersToSimulate, int MaxFriendsPerUser, int UsersPerTick, int MaxFriendsPerTick);