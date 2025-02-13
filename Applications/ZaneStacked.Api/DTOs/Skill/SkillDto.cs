namespace ZaneStacked.Api.DTOs.Skill;

public record SkillDto(
    int Id,
    string Name,
    int YearsOfExperience,
    string Proficiency
);