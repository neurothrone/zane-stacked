namespace ZaneStacked.Shared.DTOs;

public record ProjectDto(
    int Id,
    string Name,
    string Description,
    string GitHubUrl,
    string FeaturedImage,
    string? DemoUrl,
    List<SkillDto> Skills,
    DateTime CreatedDate
);