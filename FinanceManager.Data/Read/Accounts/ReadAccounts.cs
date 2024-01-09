using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Middleware.UserContext;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Read.Accounts;

public interface IReadAccounts : IRead<Account> { };
public class ReadAccounts(DataContext db, IUserContextService userContextService) : IReadAccounts
{
    private readonly Guid? _userId = userContextService.GetCurrentUserId();
    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        if (_userId == null)
        {
            return Enumerable.Empty<Account>();
        }

        return await db.Account
            .Include(x => x.BudgetTransactions)
            .Where(x => x.User.Id == _userId)
            .ToListAsync();
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        if (_userId == null)
        {
            return null;
        }

        return await db.Account
            .Include(x => x.BudgetTransactions)
            .FirstOrDefaultAsync(x => x.User.Id == _userId && x.Id == id);
    }
}