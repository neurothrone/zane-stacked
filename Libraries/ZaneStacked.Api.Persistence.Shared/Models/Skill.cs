namespace ZaneStacked.Api.Persistence.Shared.Models;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public string Proficiency { get; set; } = string.Empty;
    public List<Project> Projects { get; set; } = [];
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}