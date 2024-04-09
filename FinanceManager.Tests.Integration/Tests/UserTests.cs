using System.Net;
using FinanceManager.Api.Endpoints.Users;
using FinanceManager.Tests.Integration.Setup;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Tests.Integration.Tests;

public class UserTests(IntegrationTestApplicationFactory factory, SharedContainerFixture _) : BaseIntegrationTest(factory)
{
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
}