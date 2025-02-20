using System.Net.Http.Json;
using ZaneStacked.Shared.DTOs;

namespace ZaneStacked.Web.Services;

public class SkillService
{
    private readonly HttpClient _httpClient;

    public SkillService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Api");
    }

    public async Task<SkillDto?> AddSkillAsync(InputSkillDto skill)
    {
        var response = await _httpClient.PostAsJsonAsync("api/skills", skill);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<SkillDto>() : null;
    }

    public async Task<List<SkillDto>> GetSkillsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<SkillDto>>("api/skills") ?? [];
    }

    public async Task<SkillDto?> UpdateSkillAsync(int id, InputSkillDto skill)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/skills/{id}", skill);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<SkillDto>() : null;
    }

    public async Task<bool> DeleteSkillAsync(int skillId)
    {
        var response = await _httpClient.DeleteAsync($"api/skills/{skillId}");
        return response.IsSuccessStatusCode;
    }
}