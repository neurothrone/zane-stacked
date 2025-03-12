using ZaneStacked.Api.Persistence.Shared.Models;
using ZaneStacked.Shared.DTOs;

namespace ZaneStacked.Api.Utils;

public static class ProjectExtensions
{
    public static ProjectDto ToDto(this Project project)
    {
        return new ProjectDto(
            project.Id,
            project.Name,
            project.Description,
            project.GitHubUrl,
            project.FeaturedImage,
            project.DemoUrl,
            project.Skills.Select(skill => skill.ToDto()).ToList(),
            project.CreatedDate
        );
    }

    public static Project ToModel(this InputProjectDto dto, int id = 0)
    {
        return new Project
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            GitHubUrl = dto.GitHubUrl,
            FeaturedImage = dto.FeaturedImage,
            DemoUrl = dto.DemoUrl,
            CreatedDate = dto.CreatedDate ?? DateTime.UtcNow
        };
    }
}