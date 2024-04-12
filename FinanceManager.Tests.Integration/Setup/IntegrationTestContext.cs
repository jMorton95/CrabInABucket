using System.Net.Http.Headers;
using FinanceManager.Api.Endpoints.Auth;
using FinanceManager.Common.Constants;
using FinanceManager.Common.Models;
using FinanceManager.Data;

namespace FinanceManager.Tests.Integration.Setup;

[CollectionDefinition("Integration Test Collection")]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestContext>;

public class IntegrationTestContext : IAsyncLifetime
{
    private IServiceScope Scope { get; }
    public DataContext Db { get; }
    public HttpClient HttpClient { get; }
    public TestAuthContext AuthContext { get; }
    
    public IntegrationTestContext()
    {
        var factory = new IntegrationTestApplicationFactory();
        Scope = factory.Services.CreateScope();
        Db = Scope.ServiceProvider.GetRequiredService<DataContext>();
        HttpClient = factory.CreateClient();
        AuthContext = new TestAuthContext(HttpClient);
    }
    
    public class TestAuthContext(HttpClient client)
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

    public Task InitializeAsync()
    {
        return AuthContext.ConfigureAuthenticationContext();
    }

    public Task DisposeAsync()
    {
        Scope.Dispose();
        HttpClient.Dispose();
        return Db.DisposeAsync().AsTask();
    }
}



