using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Core.Models;
using FinanceManager.Data.Read.Users;
using FinanceManager.Data.Write.Users;
using FinanceManager.Services.Services.Interfaces;
using FinanceManager.Services.Workers.Interfaces;

namespace FinanceManager.Services.Workers;

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
            ? new CreateUserResponse(true, $"Successfully created account for {request.Username}")
            : new CreateUserResponse(false, $"Error occurred creating account for {request.Username}");
    }
}