using System.Security.Authentication;
using FinanceManager.Common.DataEntities;
using FinanceManager.Common.Middleware.UserContext;
using FinanceManager.Common.Requests;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Write.Accounts;

public interface IWriteAccounts : ICreateEntity<Account>, IEditEntity<EditAccountRequest> { }

public sealed class WriteAccounts(DataContext db, IUserContextService userContextService) : IWriteAccounts
{
    private readonly Guid? _userId = userContextService.GetCurrentUserId();
    public async Task<bool> CreateAsync(Account entity)
    {
        if (_userId == null)
        {
            //TODO: Integrate logger.
            throw new AuthenticationException("Error accessing User Context");
        }
        
        entity.User.Id = _userId.Value;
        
        db.Account.Add(entity);
        
        var saveResult = await db.SaveChangesAsync().ConfigureAwait(false);

        return saveResult > 0;
    }
    
    public async Task<bool> EditAsync(EditAccountRequest request)
    {
        var account = await db.Account.FirstOrDefaultAsync(x => x.Id == request.Id && x.User.Id == _userId).ConfigureAwait(false);
         
        if (account == null)
        {
            //TODO: Integrate logger.
            throw new InvalidOperationException("Account does not exist");
        }
        
        account.Name = request.AccountName;

        var saveResult = await db.SaveChangesAsync().ConfigureAwait(false);

        return saveResult > 0;
    }
}