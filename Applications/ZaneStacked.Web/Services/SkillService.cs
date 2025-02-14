using System.Net.Http.Json;
using ZaneStacked.Shared.DTOs;

namespace ZaneStacked.Web.Services;

public class SkillService
{
    private readonly HttpClient _httpClient;

    public SkillService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Auth");
    }

    public async Task<List<SkillDto>> GetSkillsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<SkillDto>>("api/skills") ?? [];
    }
}