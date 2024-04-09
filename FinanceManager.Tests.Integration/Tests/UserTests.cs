using System.Net;
using FinanceManager.Api.Endpoints.Users;
using FinanceManager.Common.Constants;
using FinanceManager.Tests.Integration.Setup;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Tests.Integration.Tests;

public class UserTests(IntegrationTestApplicationFactory factory, SharedContainerFixture _) : BaseIntegrationTest(factory)
{
    public static List<object[]> GetUserByEmailValidationErrorData =
    [
        [new GetByEmail.Request("plainaddress")],
        [new GetByEmail.Request("@missingusername.com")],
        // [new GetByEmail.Request("email@domain@domain.com")],
        // [new GetByEmail.Request("email@-domain.com")],
        // [new GetByEmail.Request("email@111.222.333.44444")],
        // [new GetByEmail.Request("email@domain..com")]
    ];

    public static List<object[]> TestGetUserByEmailValid = [
        [new GetByEmail.Request(TestConstants.Username)],
        [new GetByEmail.Request("does-not-exist@email.com")]
    ];
    
    [Fact]
    public async Task TestAdminGetAllUsers()
    {
        var response = await HttpClient.GetAsync("/api/users/get");
        
        Assert.Contains([HttpStatusCode.OK, HttpStatusCode.NotFound], code => code == response.StatusCode);

        var numberOfUsers = await DataContext.User.CountAsync();
        
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
        var response = await HttpClient.GetAsync($"/api/users/get/{request.Email}");
        var problemResult = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(problemResult?.Errors);
        Assert.Contains(problemResult.Errors, error => error.Key == "Email");
    }

    [Theory, MemberData(nameof(TestGetUserByEmailValid))]
    public async Task TestGetUserByEmail(GetByEmail.Request request)
    {
        var response = await HttpClient.GetAsync($"/api/users/get/{request.Email}");
        var user = await DataContext.User.FirstOrDefaultAsync(x => x.Username == request.Email);
        
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
}