using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;

namespace FinanceManager.Services.Handlers;

public interface ICreateRecurringTransactionHandler
{
    Task<BasePostResponse> Create(CreateRecurringTransactionRequest request);
}

public class CreateRecurringTransactionHandler
{
    
}