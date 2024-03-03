using FinanceManager.Api.Endpoints.Private;
using FinanceManager.Api.Endpoints.Public;

namespace FinanceManager.Api.Endpoints;

public static class EndpointMapping
{
    public static void MapApiEndpoints(this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapUserEndpoints();
        app.MapAccountEndpoints();
        app.MapTransactionEndpoints();
    }
}
