using ZaneStacked.Api.DTOs;
using ZaneStacked.Api.Persistence.Shared.Models;

namespace ZaneStacked.Api.Utils;

public static class SkillExtensions
{
    public static SkillDto ToDto(this Skill skill)
    {
        return new SkillDto(
            skill.Id,
            skill.Name,
            skill.YearsOfExperience,
            skill.Proficiency
        );
    }

    public static Skill ToModel(this InputSkillDto dto, int id = 0)
    {
        return new Skill
        {
            Id = id,
            Name = dto.Name,
            YearsOfExperience = dto.YearsOfExperience,
            Proficiency = dto.Proficiency
        };
    }
}