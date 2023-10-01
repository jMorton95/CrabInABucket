using CrabInABucket.Api.Requests;
using CrabInABucket.Api.Responses;
using CrabInABucket.Core.Services.Interfaces;
using CrabInABucket.Core.Workers.Interfaces;
using CrabInABucket.Data.Models;
using CrabInABucket.Data.Read.Users;
using CrabInABucket.Data.Write.Users;

namespace CrabInABucket.Core.Workers;

public class CreateUserWorker(IReadUsers read, IWriteUsers write, IPasswordService passwordService) : ICreateUserWorker
{
    public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
    {
        if (await read.CheckUserExistsByEmail(request.Username))
        {
            return new CreateUserResponse(false, "Account with that Email Address already exists.");
        }

        var password = passwordService.HashPassword(request.Password);
        
        var createResult = await write.CreateAsync(new User { Username = request.Username, Password = password });

        return createResult > 0
            ? new CreateUserResponse(true)
            : new CreateUserResponse(false, $"Error occurred creating account for {request.Username}");
    }
}