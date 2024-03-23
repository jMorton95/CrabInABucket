using FinanceManager.Common.Requests;
using FinanceManager.Common.Responses;
using FinanceManager.Data.Read.Accounts;
using FinanceManager.Data.Write.Accounts;

namespace FinanceManager.Services.Handlers;

public interface IEditAccountHandler
{
    Task<BasePostResponse> ChangeAccountName(EditAccountRequest req);

}

public class EditAccountHandler(IWriteAccounts write) : IEditAccountHandler
{
    public async Task<BasePostResponse> ChangeAccountName(EditAccountRequest req)
    {
        var result = await write.EditAsync(req);

        return new BasePostResponse(Success: result, Message: result ? "" : $"Error editing account.");
    }
}