using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Data.Write.Transactions;

namespace FinanceManager.Services.Handlers;

public interface ICreateDepositHandler
{
    Task<BasePostResponse> Deposit(DepositRequest req);
}

public class CreateDepositHandler(IWriteTransaction write) : ICreateDepositHandler
{
    public async Task<BasePostResponse> Deposit(DepositRequest req)
    {
        var result = await write.CreateAsync(ConvertToEntity(req));

        return new BasePostResponse(result, result ? "" : "Error occurred during deposit.");
    }

    private Transaction ConvertToEntity(DepositRequest req) 
        => new() {
            Amount = req.Amount,
            RecurringTransaction = false,
            RecipientAccountId = req.RecipientAccountId,
            SenderAccountId = req.RecipientAccountId
        };
}