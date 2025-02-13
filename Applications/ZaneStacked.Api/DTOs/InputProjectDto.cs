using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZaneStacked.Api.DTOs;

public record InputProjectDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    [DefaultValue("Project Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MinLength(10)]
    [MaxLength(500)]
    [DefaultValue("Project Description")]
    public string Description { get; set; } = string.Empty;
    
    [Url]
    [DefaultValue("https://github.com/username/repository")]
    public string GitHubUrl { get; set; } = string.Empty;
    
    [Url]
    [DefaultValue("https://demo.example.com")]
    public string DemoUrl { get; set; } = string.Empty;
    
    [DefaultValue("https://example.com/featured-image.png")]
    public string FeaturedImage { get; set; } = string.Empty;
};
