using FinanceManager.Data;

namespace FinanceManager.Tests.Integration;

public abstract class BaseIntegrationTest :
    IClassFixture<IntegrationTestApplicationFactory>,
    IClassFixture<SharedContainerFixture>,
    IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly DataContext _dataContext;
    
    protected BaseIntegrationTest(IntegrationTestApplicationFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _dataContext = _scope.ServiceProvider
            .GetRequiredService<DataContext>();
    }

    public void Dispose()
    {
        _scope.Dispose();
        _dataContext.Dispose();
    }
}