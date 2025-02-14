using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZaneStacked.Api.DTOs.Auth;

public record AuthRegisterDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    [DefaultValue("admin")]
    public required string Username { get; init; }

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    [DefaultValue("admin@example.com")]
    public required string Email { get; init; }

    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    [DefaultValue("SecurePass123!")]
    public required string Password { get; init; }
}