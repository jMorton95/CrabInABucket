using FinanceManager.Common.Requests;
using FinanceManager.Common.Responses;
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
            amount: req.Amount,
            recurringTransaction: false,
            recipientAccountId: req.RecipientAccountId,
            senderAccountId: req.RecipientAccountId
        );
        
        var result = await write.CreateAsync(transaction);

        return new BasePostResponse(result, result ? "" : "Error occurred during deposit.");
    }
}



