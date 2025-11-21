using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Auth.Dtos;

public class RefreshTokenRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    [MinLength(10)]
    public string RefreshToken { get; set; } = string.Empty;
}
