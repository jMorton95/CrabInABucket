using Testcontainers.PostgreSql;

namespace FinanceManager.Tests.Integration;

public class SharedContainerFixture : IAsyncLifetime
{
    public static PostgreSqlContainer? DatabaseContainer { get; private set; }

    public async Task InitializeAsync()
    {
        if (DatabaseContainer != null)
        {
            return;
        }
        
        DatabaseContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithExposedPort("7004")
            .Build();

        await DatabaseContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        if (DatabaseContainer == null)
        {
            return;
        }

        await DatabaseContainer.StopAsync();
        DatabaseContainer = null;
    }
}