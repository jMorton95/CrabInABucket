using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Data.Write.Transactions;
using FinanceManager.Services.Domain;

namespace FinanceManager.Services.Handlers;

public interface ICreateDepositHandler
{
    Task<BasePostResponse> Deposit(CreateDepositRequest req);
}

public class CreateDepositHandler(IWriteTransaction write, IBuildTransactionService service) : ICreateDepositHandler
{
    public async Task<BasePostResponse> Deposit(CreateDepositRequest req)
    {
        var transaction = service.MapTransaction(
            req.Amount,
            recurringTransaction: false,
            req.RecipientAccountId,
            req.RecipientAccountId
        );
        
        var result = await write.CreateAsync(transaction);

        return new BasePostResponse(result, result ? "" : "Error occurred during deposit.");
    }
}



