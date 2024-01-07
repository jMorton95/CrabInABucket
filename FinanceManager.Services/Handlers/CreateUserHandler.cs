using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Data.Read.Users;
using FinanceManager.Data.Write.Users;
using FinanceManager.Services.Services;

namespace FinanceManager.Services.Handlers;

public interface ICreateUserHandler
{
    Task<BasePostResponse> CreateUser(CreateUserRequest request);
}
public class CreateUserHandler(IReadUsers read, IWriteUsers write, IPasswordService passwordService) : ICreateUserHandler
{
    public async Task<BasePostResponse> CreateUser(CreateUserRequest request)
    {
        if (await read.CheckUserExistsByEmail(request.Username))
        {
            return new BasePostResponse(false, "Account with that Email Address already exists.");
        }

        var password = passwordService.HashPassword(request.Password);
        
        var createResult = await write.CreateAsync(new User { Username = request.Username, Password = password });

        return createResult > 0
            ? new BasePostResponse(true, $"Successfully created account for {request.Username}")
            : new BasePostResponse(false, $"Error occurred creating account for {request.Username}");
    }
}