namespace FinanceManager.Core.DataEntities;

public class UserRole : BaseModel
{
    public User? User { get; init; }
    public Role? Role { get; init; }
}