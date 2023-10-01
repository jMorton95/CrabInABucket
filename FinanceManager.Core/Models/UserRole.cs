using FinanceManager.Core.Models;

namespace FinanceManager.Core.Models;

public class UserRole : BaseModel
{
    public User? User { get; init; }
    public Role? Role { get; init; }
}