using CrabInABucket.Api.Endpoints.Private;
using CrabInABucket.Api.Endpoints.Public;

namespace CrabInABucket.Application.EndpointMapping;

public static class EndpointMapping
{
    public static void MapApiEndpoints(this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapUserEndpoints();
    }
}