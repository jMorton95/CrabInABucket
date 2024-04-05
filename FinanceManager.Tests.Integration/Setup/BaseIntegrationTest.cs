using System.Net.Http.Headers;
using FinanceManager.Api.Endpoints.Auth;
using FinanceManager.Common.Models;
using FinanceManager.Data;

namespace FinanceManager.Tests.Integration.Setup;

public abstract class BaseIntegrationTest :
    IClassFixture<IntegrationTestApplicationFactory>,
    IClassFixture<SharedContainerFixture>,
    IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly DataContext DataContext;
    protected readonly HttpClient HttpClient;
    protected readonly TestAuthContext AuthContext;
    
    protected BaseIntegrationTest(IntegrationTestApplicationFactory factory)
    {
        _scope = factory.Services.CreateScope();
        DataContext = _scope.ServiceProvider.GetRequiredService<DataContext>();
        HttpClient = factory.CreateClient();
        AuthContext = new TestAuthContext(HttpClient);
    }
    
    public void Dispose()
    {
        _scope.Dispose();
        DataContext.Dispose();
        HttpClient.Dispose();
    }
    
    protected class TestAuthContext(HttpClient client)
    {
        private TokenWithExpiry? CurrentToken { get; set; }

        public async Task ConfigureAuthenticationContext()
        {
            if (CurrentToken is not null && new DateTime(CurrentToken.ExpiryDate) <= DateTime.UtcNow)
            {
                return;
            }

            var request = new Login.Request(TestConstants.Username, TestConstants.Password);
            var loginResponse = await client.PostAsJsonAsync("/api/auth/login", request);

            if (loginResponse is not { IsSuccessStatusCode: true })
            {
                throw new Exception("Error configuring test authentication method.");
            }

            var content = await loginResponse.Content.ReadFromJsonAsync<Login.Response>();
            
            CurrentToken = content?.AccessToken;
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentToken?.Token);
        }
    }
}



