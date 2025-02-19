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

    public async Task<SkillDto?> AddSkillAsync(SkillDto skill)
    {
        var response = await _httpClient.PostAsJsonAsync("api/skills", skill);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<SkillDto>() : null;
    }

    public async Task<List<SkillDto>> GetSkillsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<SkillDto>>("api/skills") ?? [];
    }

    public async Task<SkillDto?> UpdateSkillAsync(SkillDto skill)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/skills/{skill.Id}", skill);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<SkillDto>() : null;
    }

    public async Task<bool> DeleteSkillAsync(int skillId)
    {
        var response = await _httpClient.DeleteAsync($"api/skills/{skillId}");
        return response.IsSuccessStatusCode;
    }
}