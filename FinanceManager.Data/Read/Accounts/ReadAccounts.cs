using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Middleware.UserContext;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Read.Accounts;

public interface IReadAccounts : IGetAllEntitiesAsync<Account>, IGetEntityByIdAsync<Account>
{
    Task<bool?> DoesAccountExist(string accountName);
};
public sealed class ReadAccounts(DataContext db, IUserContextService userContextService) : IReadAccounts
{
    private readonly Guid? _userId = userContextService.GetCurrentUserId();
    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        if (_userId == null)
        {
            return Enumerable.Empty<Account>();
        }

        return await db.Account
            .Include(x => x.RecurringTransactions)
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
            .Include(x => x.RecurringTransactions)
            .FirstOrDefaultAsync(x => x.User.Id == _userId && x.Id == id);
    }

    public async Task<bool?> DoesAccountExist(string accountName)
    {
        if (_userId == null) return null;
        
        return await db.Account.AnyAsync(x => x.User.Id == _userId && x.Name == accountName);
    }
}