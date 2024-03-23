using FinanceManager.Common.DataEntities;

namespace FinanceManager.Data.Write.Transactions;

public interface IWriteTransaction : ICreateEntity<Transaction> { }

public sealed class WriteTransaction(DataContext db) : IWriteTransaction
{
    public async Task<bool> CreateAsync(Transaction entity)
    {
        db.Transaction.Add(entity);
        
        var saveResult = await db.SaveChangesAsync().ConfigureAwait(false);

        return saveResult > 0;       
    }
}