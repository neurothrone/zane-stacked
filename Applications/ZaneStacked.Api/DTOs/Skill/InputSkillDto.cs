using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZaneStacked.Api.DTOs.Skill;

public record InputSkillDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    [DefaultValue(".NET")]
    public string Name { get; init; } = string.Empty;

    [Range(0, 35)]
    [DefaultValue(0)]
    public int YearsOfExperience { get; init; }

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    [DefaultValue("Proficiency Level")]
    public string Proficiency { get; init; } = string.Empty;
};