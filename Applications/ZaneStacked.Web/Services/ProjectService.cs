using System.Net.Http.Json;
using ZaneStacked.Shared.DTOs;

namespace ZaneStacked.Web.Services;

public class ProjectService
{
    private readonly HttpClient _httpClient;

    public ProjectService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Auth");
    }
    
    public async Task<ProjectDto?> AddProjectAsync(ProjectDto project)
    {
        var response = await _httpClient.PostAsJsonAsync("api/projects", project);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<ProjectDto>() : null;
    }

    public async Task<List<ProjectDto>> GetProjectsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ProjectDto>>("api/projects") ?? [];
    }
    
    public async Task<ProjectDto?> UpdateProjectAsync(ProjectDto project)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/projects/{project.Id}", project);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<ProjectDto>() : null;
    }
    
    public async Task<bool> DeleteProjectAsync(int projectId)
    {
        var response = await _httpClient.DeleteAsync($"api/projects/{projectId}");
        return response.IsSuccessStatusCode;
    }
}