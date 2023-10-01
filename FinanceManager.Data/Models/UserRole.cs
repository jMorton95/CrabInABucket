using FinanceManager.Data.Models;

namespace FinanceManager.Data.Models;

public class UserRole : BaseModel
{
    public User? User { get; init; }
    public Role? Role { get; init; }
}