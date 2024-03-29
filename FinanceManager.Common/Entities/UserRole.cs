namespace FinanceManager.Common.DataEntities;

public class UserRole : Entity
{
    public User? User { get; init; }
    public Role? Role { get; init; }
}