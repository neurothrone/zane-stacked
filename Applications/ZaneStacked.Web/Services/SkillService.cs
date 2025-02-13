using System.Net.Http.Json;
using ZaneStacked.Shared.DTOs;

namespace ZaneStacked.Web.Services;

public class SkillService
{
    private readonly HttpClient _httpClient;

    public SkillService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<SkillDto>> GetSkillsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<SkillDto>>("api/skills") ?? [];
    }
}