using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Middleware.UserContext;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Write.Accounts;

public interface IWriteAccounts : IWrite<Account> { }

public sealed class WriteAccounts(DataContext db, IUserContextService userContextService) : IWriteAccounts
{
    private readonly Guid? _userId = userContextService.GetCurrentUserId();
    public async Task<int> CreateAsync(Account entity)
    {
        var user = await db.User.FirstOrDefaultAsync(x => x.Id == _userId);

        if (user == null)
        {
            return 0;
        }
        
        entity.User = user;
        
        db.Account.Add(entity);

        return await db.SaveChangesAsync();
    }

    public Task<int> EditAsync(Account entity)
    {
        throw new NotImplementedException();
    }
}