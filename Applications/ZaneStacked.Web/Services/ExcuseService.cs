using System.Net.Http.Json;
using ZaneStacked.Shared.DTOs;

namespace ZaneStacked.Web.Services;

public class ExcuseService
{
    private readonly HttpClient _httpClient;

    public ExcuseService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Api");
    }

    public async Task<ExcuseDto?> GetRandomExcuseAsync()
    {
        return await _httpClient.GetFromJsonAsync<ExcuseDto>("api/excuses/random");
    }
}