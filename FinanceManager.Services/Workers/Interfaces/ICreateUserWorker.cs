using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;

namespace FinanceManager.Services.Workers.Interfaces;

public interface ICreateUserWorker
{
    Task<CreateUserResponse> CreateUser(CreateUserRequest request);
}