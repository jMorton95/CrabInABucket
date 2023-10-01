using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;

namespace FinanceManager.Services.Workers.Interfaces;

public interface ILoginWorker
{
    Task<LoginResponse?> Login(LoginRequest req);
}