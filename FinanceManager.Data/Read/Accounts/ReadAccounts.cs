using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Middleware.UserContext;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Read.Accounts;

public class ReadAccounts(DataContext db, IUserContextService userContextService) : AccessUserContext(userContextService), IRead<Account>
{
    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        return UserId != null 
            ? await db.Account.Include(x => x.BudgetTransactions).Where(x => x.User.Id == UserId).ToListAsync()
            : Enumerable.Empty<Account>();
    }

    public Task<Account?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}