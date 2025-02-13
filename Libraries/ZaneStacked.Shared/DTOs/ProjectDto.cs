namespace ZaneStacked.Shared.DTOs;

public record ProjectDto(
    int Id,
    string Name,
    string Description,
    string GitHubUrl,
    string? DemoUrl,
    string FeaturedImage,
    List<SkillDto> Skills
);