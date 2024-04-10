using Testcontainers.PostgreSql;

namespace FinanceManager.Tests.Integration.Setup;

public class SharedContainerFixture : IAsyncLifetime
{
    public static PostgreSqlContainer? DatabaseContainer { get; private set; }

    static SharedContainerFixture()
    {
        DatabaseContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .Build();

        DatabaseContainer.StartAsync().GetAwaiter().GetResult();
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public async Task DisposeAsync()
    {
        if (DatabaseContainer == null)
        {
            return;
        }
        
        await DatabaseContainer.StopAsync();
    }
}