using FinanceManager.Api;
using FinanceManager.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Tests.Integration.Setup;

public class IntegrationTestApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices((_, services) =>
        {
            var descriptorType =
                typeof(DbContextOptions<DataContext>);

            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == descriptorType);

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }
            
            services.AddDbContext<DataContext>(options => 
                options.UseNpgsql(SharedContainerFixture.DatabaseContainer?.GetConnectionString()));

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContextInstance = scope.ServiceProvider.GetRequiredService<DataContext>();
                dbContextInstance.Database.Migrate();
            }
        });

        return base.CreateHost(builder);
    }
}