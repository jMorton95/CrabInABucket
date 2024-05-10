using System.Net;
using FinanceManager.Api.Endpoints.Users;
using FinanceManager.Common.Constants;
using FinanceManager.Tests.Integration.Setup;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Tests.Integration.Tests;

[Collection("Integration Test Collection")]
public class UserTests(IntegrationTestContext context)
{
    public static List<object[]> GetUserByEmailValidationErrorData =
    [
        [new GetByEmail.Request("plainaddress")],
        [new GetByEmail.Request("@missingusername.com")]
    ];

    public static List<object[]> TestGetUserByEmailValid = [
        [new GetByEmail.Request(TestConstants.Username)],
        [new GetByEmail.Request("does-not-exist@email.com")]
    ];

    public static List<object[]> TestChangeUserAdminRoleMissing = [
        [new ChangeAdminRole.Request(Guid.NewGuid(), true)],
        [new ChangeAdminRole.Request(Guid.Empty, true)],
    ];
    
    [Fact]
    public async Task TestAdminGetAllUsers()
    {
        var response = await context.HttpClient.GetAsync("/api/users/get");
        
        Assert.Contains([HttpStatusCode.OK, HttpStatusCode.NotFound], code => code == response.StatusCode);

        var numberOfUsers = await context.Db.User.CountAsync();
        
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            Assert.Equal(0, numberOfUsers);
            return;
        }
        
        var userResults = await response.Content.ReadFromJsonAsync<GetAll.Response>();
        
        Assert.NotNull(userResults);
        Assert.NotEmpty(userResults.Users);
        Assert.Equal(userResults.Users.Count, numberOfUsers);
    }

    [Theory, MemberData(nameof(GetUserByEmailValidationErrorData))]
    public async Task TestGetUserByEmailValidation(GetByEmail.Request request)
    {
        var response = await context.HttpClient.GetAsync($"/api/users/get/{request.Email}");
        var problemResult = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(problemResult?.Errors);
        Assert.Contains(problemResult.Errors, error => error.Key == "Email");
    }

    [Theory, MemberData(nameof(TestGetUserByEmailValid))]
    public async Task TestGetUserByEmail(GetByEmail.Request request)
    {
        var response = await context.HttpClient.GetAsync($"/api/users/get/{request.Email}");
        var user = await context.Db.User.FirstOrDefaultAsync(x => x.Username == request.Email);
        
        Assert.Contains([HttpStatusCode.OK, HttpStatusCode.NotFound], code => code == response.StatusCode);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            Assert.Null(user);
            return;
        }
        
        var userResult = await response.Content.ReadFromJsonAsync<GetByEmail.Response>();

        Assert.NotNull(userResult);
        Assert.Equal(userResult.User.Username, request.Email);
    }

    [Theory, MemberData(nameof(TestChangeUserAdminRoleMissing))]
    public async Task TestChangeUserAdminRoleUserMissing(ChangeAdminRole.Request request)
    {
        var response = await context.HttpClient.PostAsJsonAsync("/api/users/change-admin-role", request);
        var user = await context.Db.User.FirstOrDefaultAsync(x => x.Id == request.UserId);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Null(user);
    }

    [Fact]
    public async Task TestAddUserAdminRole()
    {
        var userCount = await context.Db.User.CountAsync();
        var index = new Random().Next(0, userCount);
        var userToBecomeAdmin = await context.Db.User.ElementAtAsync(index);

        var response = await context.HttpClient.PostAsJsonAsync(
            "/api/users/change-admin-role",
            new ChangeAdminRole.Request(userToBecomeAdmin.Id, true)
        );

        var responseBody = await response.Content.ReadFromJsonAsync<ChangeAdminRole.Response>();
        
        var user = await context.Db.User
            .Include(x => x.Roles)
            .ThenInclude(y => y.Role)
            .FirstAsync(x => x.Id == userToBecomeAdmin.Id);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseBody);
        Assert.True(responseBody.Success);
        Assert.NotNull(user);
        Assert.Contains(user.Roles, role => role?.Role?.Name == PolicyConstants.AdminRole);
    }
    
    [Fact]
    public async Task TestRemoveUserAdminRole()
    {
        var userCount = await context.Db.User.CountAsync();
        var index = new Random().Next(1, userCount - 1);
        var userToRemoveAdmin = await context.Db.User.ElementAtAsync(index);
    
        var response = await context.HttpClient.PostAsJsonAsync(
            "/api/users/change-admin-role",
            new ChangeAdminRole.Request(userToRemoveAdmin.Id, false)
        );
    
        var responseBody = await response.Content.ReadFromJsonAsync<ChangeAdminRole.Response>();
        
        var user = await context.Db.User
            .Include(x => x.Roles)
            .ThenInclude(y => y.Role)
            .FirstAsync(x => x.Id == userToRemoveAdmin.Id);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseBody);
        Assert.True(responseBody.Success);
        Assert.NotNull(user);
        Assert.DoesNotContain(user.Roles, role => role?.Role?.Name == PolicyConstants.AdminRole);
    }
}