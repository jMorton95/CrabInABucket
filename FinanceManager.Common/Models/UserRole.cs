using FinanceManager.Common.Models;

namespace FinanceManager.Common.Models;

public class UserRole : BaseModel
{
    public User? User { get; init; }
    public Role? Role { get; init; }
}