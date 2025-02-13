using System.Net.Http.Json;
using ZaneStacked.Shared.DTOs;

namespace ZaneStacked.Web.Services;

public class ProjectService
{
    private readonly HttpClient _httpClient;

    public ProjectService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProjectDto>> GetProjectsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ProjectDto>>("api/projects") ?? [];
    }
}