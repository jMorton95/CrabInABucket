using FinanceManager.Common.Entities;

namespace FinanceManager.Data.Write.Transactions;

public interface IWriteRecurringTransaction : ICreateEntity<RecurringTransaction> { }

public class WriteRecurringTransaction(DataContext db) : IWriteRecurringTransaction
{
    public async Task<bool> CreateAsync(RecurringTransaction entity)
    {
        db.RecurringTransaction.Add(entity);

        var result = await db.SaveChangesAsync();

        return result > 0;
    }
}