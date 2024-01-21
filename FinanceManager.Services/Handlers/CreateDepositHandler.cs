using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Data.Write.Transactions;

namespace FinanceManager.Services.Handlers;

public interface ICreateDepositHandler
{
    Task<BasePostResponse> Deposit(CreateDepositRequest req);
}

public class CreateDepositHandler(IWriteTransaction write) : ICreateDepositHandler
{
    public async Task<BasePostResponse> Deposit(CreateDepositRequest req)
    {
        var result = await write.CreateAsync(ConvertToEntity(req));

        return new BasePostResponse(result, result ? "" : "Error occurred during deposit.");
    }

    private Transaction ConvertToEntity(CreateDepositRequest req) 
        => new() {
            Amount = req.Amount,
            RecurringTransaction = false,
            RecipientAccountId = req.RecipientAccountId,
            SenderAccountId = req.RecipientAccountId
        };
}