using FinanceManager.Common.DataEntities;
using FinanceManager.Common.Middleware.UserContext;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Read.Accounts;

public interface IReadAccounts : IGetAllOwnedEntitiesAsync<Account>, IGetOwnedEntityByIdAsync<Account>
{
    Task<bool> DoesUserAccountExist(string accountName, Guid userId);
};
public sealed class ReadAccounts(DataContext db) : IReadAccounts
{
    public async Task<IEnumerable<Account>> GetAllOwnedEntitiesAsync(Guid userId)
        => await db.Account
            .Include(x => x.RecurringTransactions)
            .Where(x => x.User.Id == userId)
            .ToListAsync();
    

    public async Task<Account?> GetOwnedEntityByIdAsync(Guid id, Guid userId)
        => await db.Account
            .Include(x => x.RecurringTransactions)
            .FirstOrDefaultAsync(x => x.User.Id == userId && x.Id == id);
    

    public async Task<bool> DoesUserAccountExist(string accountName, Guid userId)
        => await db.Account
            .AnyAsync(x => x.User.Id == userId && x.Name == accountName);
}