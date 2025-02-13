using ZaneStacked.Api.DTOs.Skill;

namespace ZaneStacked.Api.DTOs.Project;

public record ProjectDto(
    int Id,
    string Name,
    string Description,
    string GitHubUrl,
    string? DemoUrl,
    string FeaturedImage,
    List<SkillDto> Skills
);