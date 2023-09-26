using CrabInABucket.Api.Requests;
using CrabInABucket.Api.Responses;

namespace CrabInABucket.Core.Workers.Interfaces;

public interface ILoginWorker
{
    Task<LoginResponse?> Login(LoginRequest req);
}