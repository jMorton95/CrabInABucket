using FinanceManager.Api.Endpoints.Private;
using FinanceManager.Api.Endpoints.Public;
using FinanceManager.Api.Endpoints.Test;
using Microsoft.Extensions.Options;

namespace FinanceManager.Api.Endpoints;

public static class EndpointMapping
{
    public static void MapApiEndpoints(this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapUserEndpoints();
        app.MapAccountEndpoints();
        app.MapTransactionEndpoints();
        app.MapFriendshipEndpoints();
        
    }

    public static void MapDevelopmentEndpoints(this WebApplication app)
    {
        app.MapTestEndpoints();
    }
}
