using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Requests;
using FinanceManager.Data.Write.Transactions;

namespace FinanceManager.Services.Handlers;

public interface IDepositHandler
{
    Task<bool> HandleDeposit(DepositRequest req);
}

public class DepositHandler(IWriteTransaction write) : IDepositHandler
{
    public async Task<bool> HandleDeposit(DepositRequest req)
        => await write.CreateAsync(ConvertToEntity(req));
    
    private static Transaction ConvertToEntity(DepositRequest req) 
        => new() {
            Amount = req.Amount,
            RecurringTransaction = false,
            RecipientAccountId = req.RecipientAccountId,
            SenderAccountId = req.RecipientAccountId
        };
}