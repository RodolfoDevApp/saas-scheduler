using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Users.Dtos;

public class CreateUserRequest
{
    [Required]
    public Guid TenantId { get; set; }

    [Required]
    [MinLength(2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string? Phone { get; set; }

    [Required]
    [MinLength(4)]
    public string Password { get; set; } = string.Empty;
}
