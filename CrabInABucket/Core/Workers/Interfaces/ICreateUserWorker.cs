using CrabInABucket.Api.Requests;
using CrabInABucket.Api.Responses;

namespace CrabInABucket.Core.Workers.Interfaces;

public interface ICreateUserWorker
{
    Task<CreateUserResponse> CreateUser(CreateUserRequest request);
}