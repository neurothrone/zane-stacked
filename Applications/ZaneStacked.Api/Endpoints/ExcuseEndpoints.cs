using ZaneStacked.Shared.DTOs;

namespace ZaneStacked.Api.Endpoints;

public static class ExcuseEndpoints
{
    public static void MapExcuseEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/excuses")
            .WithTags("Excuses");

        group.MapGet("/random", async (IHttpClientFactory httpClientFactory) =>
        {
            var httpClient = httpClientFactory.CreateClient("ExcusesApi");
            return await httpClient.GetFromJsonAsync<ExcuseDto>("api/v1/excuses/random");
        }).RequireAuthorization();
    }
}