using FinanceManager.Core.DataEntities;

namespace FinanceManager.Data.Read.Accounts;

public class ReadAccounts(DataContext db) : IRead<Account>
{
    public Task<IEnumerable<Account>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Account?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}