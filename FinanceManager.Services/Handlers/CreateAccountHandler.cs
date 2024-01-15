using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Data.Read.Accounts;
using FinanceManager.Data.Write.Accounts;

namespace FinanceManager.Services.Handlers;

public interface ICreateAccountHandler
{
    Task<BasePostResponse> CreateAccount(CreateAccountRequest req);
}

public class CreateAccountHandler(IReadAccounts read, IWriteAccounts write) : ICreateAccountHandler
{
    public async Task<BasePostResponse> CreateAccount(CreateAccountRequest req)
    {
        var doesAccountExist = await read.DoesAccountExist(req.AccountName);

        return (doesAccountExist) switch
        {
            null => new BasePostResponse(false, "Error accessing User Context"),
            true => new BasePostResponse(false, $"Account with name: {req.AccountName} already exists."),
            false => await CreateNewAccount(req)
        };
    }
    
    private async Task<BasePostResponse> CreateNewAccount(CreateAccountRequest req)
    {
        var creationResult = await write.CreateAsync(new Account() { Name = req.AccountName });
        
        return new BasePostResponse(creationResult, creationResult ? "" : "Could not create account");
    }
}