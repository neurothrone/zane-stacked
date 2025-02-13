namespace ZaneStacked.Api.Persistence.EFCore.Models;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string GitHubUrl { get; set; } = string.Empty;
    public string? DemoUrl { get; set; }
    public string FeaturedImage { get; set; } = string.Empty;
    public List<Skill> Skills { get; set; } = [];
}