using ZaneStacked.Api.DTOs.Project;
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
            project.DemoUrl,
            project.FeaturedImage,
            project.Skills.Select(skill => skill.ToDto()).ToList()
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
            DemoUrl = dto.DemoUrl,
            FeaturedImage = dto.FeaturedImage
        };
    }
}