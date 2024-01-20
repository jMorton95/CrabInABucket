using FinanceManager.Core.Requests;

namespace FinanceManager.Services.Handlers;

public interface IDepositHandler
{
    Task<bool> HandleDeposit(DepositRequest req);
}

public class DepositHandler : IDepositHandler
{
    public Task<bool> HandleDeposit(DepositRequest req)
    {
        throw new NotImplementedException();
    }
}